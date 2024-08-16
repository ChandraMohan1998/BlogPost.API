using BlogPost.API.Model.Domain;

namespace BlogPost.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BloggerPost> CreateBlogPost(BloggerPost bloggerPost);

        Task<IEnumerable<BloggerPost>> GetAllAsync();

        Task<BloggerPost?> GetByIdAsync(Guid id);

        Task<BloggerPost?> UpdateAsync(BloggerPost blogPost);

        Task<BloggerPost?> DeleteAsync(Guid id);

        Task<BloggerPost?> GetByUrlHandleAsync(string urlHandle);
    }
}
