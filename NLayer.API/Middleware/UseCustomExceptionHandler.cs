using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middleware
{
    public static class UseCustomExceptionHandler  //extension metodlarda class static olur
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>(); //bu interface üzerinden uygulamada fırlatılan hatayı alıyoruz

                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,  // eğer hata client bazlı ise statuscode'u 400 ata
                        NotFoundException=> 404,  
                        _ => 500  // ClientSideException ve NotFoundException dışında bir şeyse 500 ata
                    };
                    context.Response.StatusCode = statusCode;

                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                    // controllerda bir tip döndüğümüzde kendisi otomatik json a dönüyor ama burada kendimiz bir middleware oluştuduğumuz için dönüştürme işlemini de kendimiz yapmalıyız
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
