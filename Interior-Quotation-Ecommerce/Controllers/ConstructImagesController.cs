using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.MongoDB.Entities;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interior_Quotation_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstructImagesController : ControllerBase
    {
        private readonly IConstructImagesService _service;

        public ConstructImagesController(IConstructImagesService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpPost("/api/v1/images")]
        public IActionResult CreateConstructImages([FromForm] ConstructImagesPostDTO fileUpload)
        {
            try
            {
                var result = _service.CreateConstructImages(fileUpload);
                return result != null ? Ok("Created success") : BadRequest("Not correct file");
            } catch (ArgumentException ex)
            {
                return StatusCode(int.Parse(ex.ParamName), ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/images")]
        public IActionResult GetConstructImages()
        {
            try
            {
                return Ok(_service.GetConstructImages());
            }
            catch
            {
                return BadRequest("Not good");
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/images/{constructId}")]
        public IActionResult GetConstructImagesByConstructId(long  constructId)
        {
            try
            {
                var list = _service.GetConstructImagesByConstructId(constructId);
                return Ok(list);
            }
            catch
            {
                return BadRequest("Could not find");
            }
        }
    }
}
