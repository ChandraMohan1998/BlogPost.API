namespace BlogPost.API.Model.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }

        public ICollection<BloggerPost> BloggerPosts { get; set; }
    }
}
