using TannersWebsiteTemplate.Models;

namespace TannersWebsiteTemplate.Interfaces.SQL
{
    public interface IBlog
    {
        public static abstract Task AddBlogPost(string title, string message);
        public static abstract Task UpdateBlogPost(int blogid, string title, string message);
        public static abstract Task DeleteBlogPost(int blogid);
        public static abstract Task<BlogPost> GetBlogPost(int blogid);
        public static abstract Task<int> GetBlogPostCount();
        public static abstract Task<bool> DoesBlogPostExist(int blogid);
    }
}
