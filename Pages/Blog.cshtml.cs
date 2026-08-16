using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TannersWebsiteTemplate.Models;

namespace TannersWebsiteTemplate.Pages
{
    public class BlogModel : PageModel
    {
        [BindProperty]
        public string Comment { get; set; }

        [BindProperty]
        public int Post { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Message { get; set; }


        [BindProperty]
        public string Date { get; set; }

        [BindProperty]
        public CommentSection cs { get; set; }
        public async Task OnGet()
        {
            // If the RouteData.Values["post"] is a string (it always will be) and if the string is parseable to an int, set it to post, else set it to 0
            Post = RouteData.Values["post"] is string s == true && int.TryParse(s, out int post) == true ? Post = post : Post = 0;
            if (Post < 1) // if post is less than 1 we went too far back
            {
                Post = 1;
            }
            else if (Post > await SQL.Blog.GetBlogPostCount()) // if post is greater than the total amount of posts, we went too far forward
            {
                Post = await SQL.Blog.GetBlogPostCount();
            }
            if (await SQL.Blog.DoesBlogPostExist(Post) == false)
            {
                Title = "Deleted Post";
                Message = "This post was deleted";
                Date = "Unknown";
            }
            BlogPost blogpost = await SQL.Blog.GetBlogPost(Post); //  Get the post to be displayed in the page
            Title = blogpost.Title;
            Message = blogpost.Message;
            Date = blogpost.Date;
            cs = await SQL.Comments.GetCommentSection(Post);
        }

        public async Task<IActionResult> OnPost()
        {
            Post = int.TryParse(Request.Form["CS"], out int cs) ? cs : 1; // What post this comment belongs to (CS input in form on page)
            string username = await SQL.Accounts.DoesUserExist(HttpContext.Session.GetString("Username")) ? HttpContext.Session.GetString("Username") : "Anonymous"; // if the user is not logged in, use anonymous
            await SQL.Comments.AddComment(Comment, username, Post);

            return RedirectToPage("/Blog", new { post = Post});
        }

        public async Task<IActionResult> OnPostDelete(int? commentid)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && await SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId")))
            {
                await SQL.Comments.DeleteComment(commentid);
            }

            return RedirectToPage("/Blog", new { post = Post });
        }
    }
}
