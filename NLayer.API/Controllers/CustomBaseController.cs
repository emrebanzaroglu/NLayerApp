using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]  //endpoint olmadığını belli etmek için yazıyoruz. get, post vs yazmadığımız için swagger hata fırlatır 
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response.StatusCode == 204) //204 durum kodu (NoContent) geriye bir şey dönmeyeceksiniz anlamına geliyor 
                return new ObjectResult(null) //204 için null verdik
                {
                    StatusCode = response.StatusCode
                };
            return new ObjectResult(response) //204 harici durum kodları için response
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
