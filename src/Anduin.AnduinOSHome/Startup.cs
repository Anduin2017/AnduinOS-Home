using System.Globalization;
using Aiursoft.Scanner;
using Aiursoft.WebTools.Abstractions.Models;
using Anduin.AnduinOSHome.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Anduin.AnduinOSHome;

public class Startup : IWebStartup
{
    public static Dictionary<string, string> SupportedCultures = new()
    {
        { "en_US", "English (United States)" },
        { "en_GB", "English (United Kingdom)" },
        { "zh_CN", "中文 (中国大陆)" },
        { "zh_TW", "中文 (台灣)" },
        { "zh_HK", "中文 (香港)" },
        { "ja_JP", "日本語 (日本)" },
        { "ko_KR", "한국어 (대한민국)" },
        { "vi_VN", "Tiếng Việt (Việt Nam)" },
        { "th_TH", "ภาษาไทย (ประเทศไทย)" },
        { "de_DE", "Deutsch (Deutschland)" },
        { "fr_FR", "Français (France)" },
        { "es_ES", "Español (España)" },
        { "ru_RU", "Русский (Россия)" },
        { "it_IT", "Italiano (Italia)" },
        { "pt_PT", "Português (Portugal)" },
        { "pt_BR", "Português (Brasil)" },
        { "ar_SA", "العربية (المملكة العربية السعودية)" },
        { "nl_NL", "Nederlands (Nederland)" },
        { "sv_SE", "Svenska (Sverige)" },
        { "pl_PL", "Polski (Polska)" },
        { "tr_TR", "Türkçe (Türkiye)" }
    };

    public void ConfigureServices(IConfiguration configuration, IWebHostEnvironment environment, IServiceCollection services)
    {
        services.AddLibraryDependencies();

        services.Configure<List<VersionInfo>>(configuration.GetSection("Versions"));
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services
            .AddControllersWithViews()
            .AddApplicationPart(typeof(Startup).Assembly)
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();
    }

    public void Configure(WebApplication app)
    {
        var supportedCultures = Startup.SupportedCultures.Select(c => new CultureInfo(c.Key)).ToList();
        var defaultLanguage = supportedCultures.First();
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultLanguage),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();
        app.UseWebSockets();
    }
}
