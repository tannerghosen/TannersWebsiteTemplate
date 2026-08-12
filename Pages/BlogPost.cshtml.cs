using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TannersWebsiteTemplate.Pages
{
    public class BlogPostModel : PageModel
    {
        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public string BlogPostId { get; set; }

        public async Task OnGet()
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1 || !await SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId")))
            {
                Response.Redirect("/Index");
            }
        }
        public async Task<IActionResult> OnPost()
        {
            int blogid = await SQL.Blog.GetBlogPostCount() + 1;
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && await SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId")))
            {
                if(!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Message))
                {
                    await SQL.Blog.AddBlogPost(Title, Message);
                }
            }
            return RedirectToPage("/Blog", new { post = blogid });
        }

        public async Task<IActionResult> OnPostDelete()
        {
            int id = int.TryParse(Request.Form["BlogPostId"], out int blogpostid) ? blogpostid : 0;
            if (HttpContext.Session.GetInt32("IsAdmin") == 1 && await SQL.Admin.IsAdmin(HttpContext.Session.GetInt32("UserId")))
            {
                await SQL.Blog.DeleteBlogPost(id);
            }

            return RedirectToPage("/BlogPost");
        }
    }
}
