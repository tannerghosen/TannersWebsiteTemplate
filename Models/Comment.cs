namespace TannersWebsiteTemplate.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Date = "";
        public int UserId { get; set; }
        public string Username = "";
        public string Content = "";
    }
}
