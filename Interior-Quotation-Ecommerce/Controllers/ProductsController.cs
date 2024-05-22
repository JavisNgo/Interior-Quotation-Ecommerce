using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.Services.Implements;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Interior_Quotation_Ecommerce.Controllers
{
    [Route("/api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/products")]
        public IActionResult GetAll()
        {
            try
            {
                var products = _productsService.GetAll();
                return Ok(products);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/products")]
        public IActionResult CreateProduct([FromBody] ProductsPostDTO productsPostDTO)
        {
            try
            {
                var result = _productsService.AddProduct(productsPostDTO);
                if(result != null)
                {
                    return Ok(result);
                }
                return BadRequest();
            } catch (ArgumentException arex)
            {
                return StatusCode(int.Parse(arex.ParamName), arex.Message);
            }
        }
    }
}
