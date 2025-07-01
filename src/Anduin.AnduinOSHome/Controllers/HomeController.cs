using Microsoft.AspNetCore.Mvc;
using Aiursoft.WebTools.Attributes;
using Anduin.AnduinOSHome.Models;
using Microsoft.Extensions.Options;

namespace Anduin.AnduinOSHome.Controllers;

[LimitPerMin]
public class HomeController(IOptions<List<VersionInfo>> versions) : Controller
{
    private readonly List<VersionInfo> _versions = versions.Value;

    public IActionResult Index()
    {
        return View();
    }

    [Route("/versions.json")]
    public IActionResult GetVersions()
    {
        return Json(_versions);
    }

    [Route("/privacy.html")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("/terms.html")]
    public IActionResult Terms()
    {
        return View();
    }

    [Route("/thankyou.html")]
    public IActionResult ThankYou()
    {
        return View();
    }

}
