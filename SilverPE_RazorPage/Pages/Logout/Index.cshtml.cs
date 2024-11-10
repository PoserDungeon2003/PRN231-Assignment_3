using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SilverPE_RazorPage.Pages.Logout
{
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToPage("/login/index");
        }
    }
}