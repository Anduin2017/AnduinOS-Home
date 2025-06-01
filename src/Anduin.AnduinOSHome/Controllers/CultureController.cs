using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Controllers;

public class CultureController : Controller
{
    [HttpPost]
    public IActionResult Set(string culture)
    {
        if (string.IsNullOrEmpty(culture))
        {
            return BadRequest("Culture cannot be null or empty.");
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return RedirectToAction("Index", "Home");
    }
}
