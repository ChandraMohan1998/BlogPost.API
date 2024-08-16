using BlogPost.API.Model.Domain;
using BlogPost.API.Model.DTOs;
using BlogPost.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPost.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private IBlogPostRepository _blogPostRepository;
        private ICategoryRepository _categoryRepository;
        public BlogPostController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            var bloggerPost = new BloggerPost
            {
                Author = request.Author,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                PublishedDate = request.PublishedDate,
                Content = request.Content,
                UrlHandle = request.UrlHandle,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories) 
            {
                var category = await _categoryRepository.GetByIdAsync(categoryGuid);
                if (category is not null)
                {
                    bloggerPost.Categories.Add(category);
                }
            }

           bloggerPost =  await _blogPostRepository.CreateBlogPost(bloggerPost);

            var result = new BloggerPostDto
            {
                Id = bloggerPost.Id,
                Author = bloggerPost.Author,
                Title = bloggerPost.Title,
                ShortDescription = bloggerPost.ShortDescription,
                PublishedDate = bloggerPost.PublishedDate,
                Content = bloggerPost.Content,
                UrlHandle = bloggerPost.UrlHandle,
                FeaturedImageUrl = bloggerPost.FeaturedImageUrl,
                IsVisible = bloggerPost.IsVisible,
                Categories = bloggerPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var bloggerPosts = await _blogPostRepository.GetAllAsync();

            var result = new List<BloggerPostDto>();
            foreach(var bloggerPost in bloggerPosts)
            {
                result.Add(new BloggerPostDto
                {
                    Id = bloggerPost.Id,
                    Author = bloggerPost.Author,
                    Title = bloggerPost.Title,
                    ShortDescription = bloggerPost.ShortDescription,
                    PublishedDate = bloggerPost.PublishedDate,
                    Content = bloggerPost.Content,
                    UrlHandle = bloggerPost.UrlHandle,
                    FeaturedImageUrl = bloggerPost.FeaturedImageUrl,
                    IsVisible = bloggerPost.IsVisible,
                    Categories = bloggerPost.Categories.Select(x => new CategoryDto 
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var bloggerPost = await _blogPostRepository.GetByIdAsync(id);

            if (bloggerPost is null)
            {
                return NotFound();
            }

            var result = new BloggerPostDto
            {
                Id = bloggerPost.Id,
                Author = bloggerPost.Author,
                Title = bloggerPost.Title,
                ShortDescription = bloggerPost.ShortDescription,
                Content = bloggerPost.Content,
                UrlHandle = bloggerPost.UrlHandle,
                FeaturedImageUrl = bloggerPost.FeaturedImageUrl,
                PublishedDate = bloggerPost.PublishedDate,
                IsVisible = bloggerPost.IsVisible,
                Categories = bloggerPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(result);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            // Convert DTO to Domain Model
            var blogPost = new BloggerPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            // Foreach 
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);

                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }


            // Call Repository To Update BlogPost Domain Model
            var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogPost);

            if (updatedBlogPost == null)
            {
                return NotFound();
            }

            // Convert Domain model back to DTO
            var response = new BloggerPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            // Get blogpost details from repository
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            var response = new BloggerPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedBlogPost = await _blogPostRepository.DeleteAsync(id);

            if (deletedBlogPost == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new BloggerPostDto
            {
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                PublishedDate = deletedBlogPost.PublishedDate,
                ShortDescription = deletedBlogPost.ShortDescription,
                Title = deletedBlogPost.Title,
                UrlHandle = deletedBlogPost.UrlHandle
            };

            return Ok(response);
        }
    }
}
