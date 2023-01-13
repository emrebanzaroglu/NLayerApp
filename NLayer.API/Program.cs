using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middleware;
using NLayer.API.Modules;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());  //validasyonlar� burada ef ye aktar�yoruz

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter= true;  //ef nin default d�nd��� model filtresini bask�lad�k. bu sayede kendi haz�rlad���m�z filtreyi kullanabilece�iz. Bu i�lemi MVC  taraf�nda yapmam�za gerek yok ��nk� validate filter api taraf�nda default olarak aktif
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//ef core'a connection string'i kullanaca��m�z�n bilgisini veriyoruz
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);  //migrationu appdbcontext'in bulundu�u assembly'de yapmak i�in b�yle yaz�yoruz
    });
});


builder.Services.AddMemoryCache();  //caching i�lemini ekledik

builder.Services.AddScoped(typeof(NotFoundFilter<>));  //generic oldu�u i�in oklar� a��p kapad�k

builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
// repo ve service'leri tek tek yazmak yerine dinamikle�tirdik



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();  //kendi olu�turdu�um middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
