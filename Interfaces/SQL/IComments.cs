using TannersWebsiteTemplate.Models;

namespace TannersWebsiteTemplate.Interfaces.SQL
{
    public interface IComments
    {
        public static abstract Task AddComment(string? comment, string username = "Anonymous", int commentsection = 0);
        public static abstract Task<CommentSection> GetCommentSection(int section = 0);
        public static abstract Task DeleteComment(int? commentid);
        public static abstract Task<int> CountCommentsByUserId(int userid);
    }
}
