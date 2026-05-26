using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kintech.Views.Product
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public decimal Price { get; set; }

        [BindProperty]
        public string? ImageUrl { get; set; }

        [BindProperty]
        public int Stock { get; set; }

        public void OnGet()
        {
        }
    }
}
