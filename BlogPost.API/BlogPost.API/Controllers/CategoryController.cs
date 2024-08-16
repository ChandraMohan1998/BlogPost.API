using BlogPost.API.Model.DTOs;
using BlogPost.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using BlogPost.API.Model.Domain;

namespace BlogPost.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
                UrlHandle = createCategoryDto.UrlHandle,
            };

            await _categoryRepository.CreateAsync(category);

            var response = new CategoryDto { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle };
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is not null)
            {
                var response = new CategoryDto { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle };
                return Ok(response);
            }
                return NotFound();
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto updateCategoryDto)
        {
            var category = new Category
            {
                Id = id,
                Name = updateCategoryDto.Name,
                UrlHandle = updateCategoryDto.UrlHandle,
            };
            category = await _categoryRepository.UpdateAsync(category);

            if (category is not null)
            {
                var response = new CategoryDto { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle };
                return Ok(response);
            }
                return NotFound();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var category = await _categoryRepository.DeleteAsync(id);
            if (category is not null)
            {
                var response = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                };

                return Ok(response);
            }

            return NotFound("No data found with id: " + id);
        }
    }
}
