using Aiursoft.Scanner;
using Aiursoft.WebTools.Abstractions.Models;
using Anduin.AnduinOSHome.Models;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Anduin.AnduinOSHome;

public class Startup : IWebStartup
{
    public void ConfigureServices(IConfiguration configuration, IWebHostEnvironment environment, IServiceCollection services)
    {
        services.AddLibraryDependencies();

        services.Configure<List<VersionInfo>>(configuration.GetSection("Versions"));
        services
            .AddControllersWithViews()
            .AddApplicationPart(typeof(Startup).Assembly)
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();
    }

    public void Configure(WebApplication app)
    {
        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();
        app.UseWebSockets();
    }
}
