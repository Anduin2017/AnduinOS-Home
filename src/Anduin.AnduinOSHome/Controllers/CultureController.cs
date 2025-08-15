using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Controllers;

public class CultureController : Controller
{
    [HttpPost]
    public IActionResult Set(string culture, string returnUrl)
    {
        if (string.IsNullOrEmpty(culture))
            return BadRequest("Culture cannot be null or empty.");

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        // 如果 returnUrl 合法，就重定向回去；否则降级到首页
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}
