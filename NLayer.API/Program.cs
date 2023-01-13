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



builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());  //validasyonlarý burada ef ye aktarýyoruz

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter= true;  //ef nin default döndüðü model filtresini baskýladýk. bu sayede kendi hazýrladýðýmýz filtreyi kullanabileceðiz. Bu iþlemi MVC  tarafýnda yapmamýza gerek yok çünkü validate filter api tarafýnda default olarak aktif
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//ef core'a connection string'i kullanacaðýmýzýn bilgisini veriyoruz
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);  //migrationu appdbcontext'in bulunduðu assembly'de yapmak için böyle yazýyoruz
    });
});


builder.Services.AddMemoryCache();  //caching iþlemini ekledik

builder.Services.AddScoped(typeof(NotFoundFilter<>));  //generic olduðu için oklarý açýp kapadýk

builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
// repo ve service'leri tek tek yazmak yerine dinamikleþtirdik



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();  //kendi oluþturduðum middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
