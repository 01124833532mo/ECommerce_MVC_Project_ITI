using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceIti.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordModel : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("/Account/Login", new { area = "Identity" });
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("/Account/Login", new { area = "Identity" });
    }
}