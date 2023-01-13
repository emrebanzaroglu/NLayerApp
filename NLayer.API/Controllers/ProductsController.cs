using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //[Route("api/[controller]")]  // CustomBaseController'da da olduklarından gerek yok buna ve altındakine
    //[ApiController]
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService productService;
        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            this.productService = productService;
        }

        //Get api/products/getproductswithcategory  birden fazla get işlemi olduğundan metod ismini vererek ayırmak lazım  çünkü metodun ismine değil tipine göre eşleşme var
        [HttpGet("[action]")] //[action] yerine GetProductsWithCategory de yazabilirdik ama metodun ismi değişirse get işleminde de değişmek gerekirdi. Böyle daha dinamik olur.
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await productService.GetProductsWithCategory());
        }


        //GET api/products
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await productService.GetAllAsync();
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList());
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));  // böyle yaparsak Ok, BadRequest vs için ayrı ayrı yapmak gerekir
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos)); //böyle yaparsak best practice
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        //GET api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await productService.AddAsync(_mapper.Map<Product>(productDto)); //productDto yu Product'a çevirdik çünkü Product bekliyor bizden
            var productsDto = _mapper.Map<ProductDto>(product);  // geri dto ya dönüştürdük
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));  //201 durum kodu created demek
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await productService.UpdateAsync(_mapper.Map<Product>(productDto)); //geriye bir şey dönmüyoruz
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204)); // bir şey dönmediğimiz için dto yu boş gönderiyoruz
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await productService.GetByIdAsync(id);
            await productService.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
