using Interior_Quotation_Ecommerce.Models.POST;
using Interior_Quotation_Ecommerce.Services.Implements;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interior_Quotation_Ecommerce.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ConstructsController : ControllerBase
    {
        private readonly IConstructsService _constructsService;
        private readonly IConstructImagesService _constructImagesService;

        public ConstructsController(IConstructsService constructsService, IConstructImagesService constructImagesService)
        {
            _constructsService = constructsService;
            _constructImagesService = constructImagesService;
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/constructs")]
        public IActionResult CreateConstruct([FromBody] ConstructsPostDTO constructsPostDTO, [FromForm] List<ConstructImagesPostDTO> fileUploads)
        {
            try
            {
                var constructsDTO = _constructsService.CreateConstruct(constructsPostDTO, fileUploads);

                if(constructsDTO != null)
                {
                    return Ok(constructsDTO);
                }
                return BadRequest("Error occur while creating construct");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(int.Parse(ex.ParamName), ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/constructs")]
        public IActionResult GetConstructs()
        {
            try
            {
                var constructList = _constructsService.Get();
                return Ok(constructList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/constructs/{id}")]
        public IActionResult GetConstructById([FromQuery]long id)
        {
            try
            {
                var construct = _constructsService.GetConstructById(id);
                return Ok(construct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("/api/v1/constructs")]
        public IActionResult UpdateConstruct([FromBody] ConstructsPostDTO constructsPostDTO, [FromForm] List<ConstructImagesPostDTO> fileUploads)
        {
            try
            {
                var result = _constructsService.UpdateConstruct(constructsPostDTO, fileUploads);
                if( result != null )
                {
                    return Ok(result);
                }
                return BadRequest("Error occur while creating construct");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpDelete("/api/v1/constructs")]
        public IActionResult DeleteConstruct([FromQuery]long id)
        {
            try
            {
                var result = _constructsService.IsDeleteConstruct(id);
                if (result)
                {
                    return Ok("Delete success");
                }
                return BadRequest("Error occur while creating construct");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
