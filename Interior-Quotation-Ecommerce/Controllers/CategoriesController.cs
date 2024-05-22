
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Services.Implements;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Interior_Quotation_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/categories/get")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categoriesList = _categoriesService.GetCategories();
                return Ok(categoriesList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/categories/get/id={id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var category = _categoriesService.GetCategoryById(id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("/api/v1/categories/post")]
        public IActionResult AddCategory([FromQuery] string categoryName)
        {
            CategoriesDTO categoryView = new CategoriesDTO();

            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name is required");
            }
            try
            {
                if (!IsValidName(categoryName))
                {
                    return BadRequest("Invalid name. It should only contain letters.");
                }
                categoryView.Name = categoryName;

                var category = _categoriesService.AddCategory(categoryView);
                if (category == null)
                {
                    return BadRequest();
                }
                return Ok("Create successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("/api/v1/categories/put")]
        public IActionResult UpdateCategory([FromQuery] int id, [FromQuery] string updatedCategoryName)
        {
            CategoriesDTO updatedCategoryDTO = new CategoriesDTO();
            if (string.IsNullOrEmpty(updatedCategoryName))
            {
                return BadRequest("Category name is required");
            }
            try
            {
                // Validate the name using a regular expression
                if (!IsValidName(updatedCategoryName))
                {
                    return BadRequest("Invalid name. It should only contain letters.");
                }
                updatedCategoryDTO.Name = updatedCategoryName;

                var existingCategory = _categoriesService.UpdateCategory(id, updatedCategoryDTO);

                if (existingCategory == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }

                return Ok("Update successfully"); // Return the updated category
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpDelete("/api/v1/categories/delete/id={id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var category = _categoriesService.DeleteCategory(id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }

                return Ok(new { Message = $"Category with ID {id} has been successfully deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private bool IsValidName(string name)
        {
            // Use a regular expression to check if the name contains only letters and spaces
            return !string.IsNullOrWhiteSpace(name) && System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$");
        }
    }
}
