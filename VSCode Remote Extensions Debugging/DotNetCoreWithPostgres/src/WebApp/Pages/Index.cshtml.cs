using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BloggingContext _context;
        private readonly ILogger<IndexModel> _logger;
        public IList<Blog> Blogs { get; set; }
        public IList<Post> Posts { get; set; }

        public IndexModel(BloggingContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnGet()
        {
            Blogs = _context.Blogs.ToList();
            Posts = _context.Posts.ToList();
        }
    }
}
