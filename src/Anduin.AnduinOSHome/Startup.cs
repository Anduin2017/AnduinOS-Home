using System.Globalization;
using Aiursoft.Scanner;
using Aiursoft.WebTools.Abstractions.Models;
using Anduin.AnduinOSHome.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Anduin.AnduinOSHome;

public class Startup : IWebStartup
{
    public static readonly Dictionary<string, string> SupportedCultures = new()
    {
        { "zh-CN", "中文 (中国大陆)" },
        { "zh-TW", "中文 (台灣)" },
        { "zh-HK", "中文 (香港)" },
        { "ja-JP", "日本語 (日本)" },
        { "ko-KR", "한국어 (대한민국)" },
        { "vi-VN", "Tiếng Việt (Việt Nam)" },
        { "th-TH", "ภาษาไทย (ประเทศไทย)" },
        { "de-DE", "Deutsch (Deutschland)" },
        { "fr-FR", "Français (France)" },
        { "es-ES", "Español (España)" },
        { "ru-RU", "Русский (Россия)" },
        { "it-IT", "Italiano (Italia)" },
        { "pt-PT", "Português (Portugal)" },
        { "pt-BR", "Português (Brasil)" },
        { "ar-SA", "العربية (المملكة العربية السعودية)" },
        { "nl-NL", "Nederlands (Nederland)" },
        { "sv-SE", "Svenska (Sverige)" },
        { "pl-PL", "Polski (Polska)" },
        { "tr-TR", "Türkçe (Türkiye)" },
        { "ro-RO", "Română (România)" },
        { "da-DK", "Dansk (Danmark)" },
        { "uk-UA", "Українська (Україна)" },
        { "id-ID", "Bahasa Indonesia (Indonesia)" },
        { "fi-FI", "Suomi (Suomi)" },
        { "hi-IN", "हिन्दी (भारत)" },
        { "el-GR", "Ελληνικά (Ελλάδα)" }
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

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultLanguage),
            SupportedCultures      = supportedCultures,
            SupportedUICultures    = supportedCultures,
            FallBackToParentCultures    = true,
            FallBackToParentUICultures  = true,
            RequestCultureProviders =
            [
                new CookieRequestCultureProvider(),
                new StartsWithAcceptLanguageProvider()
            ]
        };

        app.UseRequestLocalization(localizationOptions);
        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();
        app.UseWebSockets();
    }
}
