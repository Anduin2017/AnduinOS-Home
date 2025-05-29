using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Views.Shared.Components.Footer;

public class Footer : ViewComponent
{
    public IViewComponentResult Invoke(FooterViewModel? model = null)
    {
        model ??= new FooterViewModel();
        return View(model);
    }
}
