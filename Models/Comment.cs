namespace TannersWebsiteTemplate.Models
{
    public class Comment
    {
        public int CommentSId { get; set; }
        public string Date { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
    }
}
