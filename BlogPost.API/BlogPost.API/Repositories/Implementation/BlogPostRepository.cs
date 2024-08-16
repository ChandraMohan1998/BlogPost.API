using BlogPost.API.Data;
using BlogPost.API.Model.Domain;
using BlogPost.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private ApplicationDbContext _dbContext;
        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BloggerPost> CreateBlogPost(BloggerPost bloggerPost)
        {
            await _dbContext.BlogPosts.AddAsync(bloggerPost);
            await _dbContext.SaveChangesAsync();

            return bloggerPost;
        }

        public async Task<IEnumerable<BloggerPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BloggerPost?> GetByIdAsync(Guid id)
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BloggerPost?> UpdateAsync(BloggerPost blogPost)
        {
            var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost == null)
            {
                return null;
            }

            // Update BlogPost
            _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await _dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BloggerPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost != null)
            {
                _dbContext.BlogPosts.Remove(existingBlogPost);
                await _dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

        public async Task<BloggerPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

    }
}
