namespace TannersWebsiteTemplate.Models
{
    public class CommentSection
    {
        public List<Comment> Comments { get; set; }

        public CommentSection()
        {
            Comments = new List<Comment>();
        }
    }
}
