using Anduin.AnduinOSHome.Models.HomeViewModels;
using Anduin.AnduinOSHome.Services;
using Aiursoft.WebTools.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Controllers;

[LimitPerMin]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.SimpleView(new IndexViewModel());
    }
}
