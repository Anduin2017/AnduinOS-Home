using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Views.Shared.Components.Navbar;

public class Navbar : ViewComponent
{
    public IViewComponentResult Invoke(NavbarViewModel? model = null)
    {
        model ??= new NavbarViewModel();
        return View(model);
    }
}
