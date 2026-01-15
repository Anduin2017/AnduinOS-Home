using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Views.Shared.Components.MarketingNavbar;

public class MarketingNavbar : ViewComponent
{
    public IViewComponentResult Invoke(MarketingNavbarViewModel? model = null)
    {
        model ??= new MarketingNavbarViewModel();
        return View(model);
    }
}
