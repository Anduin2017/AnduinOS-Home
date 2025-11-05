using Anduin.AnduinOSHome.Models.HomeViewModels;
using Anduin.AnduinOSHome.Services;
using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.SimpleView(new IndexViewModel());
    }
}
