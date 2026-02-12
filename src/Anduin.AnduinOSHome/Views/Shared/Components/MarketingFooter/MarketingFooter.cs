using Anduin.AnduinOSHome.Configuration;
using Anduin.AnduinOSHome.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;
using Anduin.AnduinOSHome.Services;

namespace Anduin.AnduinOSHome.Views.Shared.Components.MarketingFooter;

public class MarketingFooter(
    GlobalSettingsService globalSettingsService,
    StorageService storageService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(MarketingFooterViewModel? model = null)
    {
        model ??= new MarketingFooterViewModel();
        model.BrandName = await globalSettingsService.GetSettingValueAsync(SettingsMap.BrandName);
        model.BrandHomeUrl = await globalSettingsService.GetSettingValueAsync(SettingsMap.BrandHomeUrl);
        model.Icp = await globalSettingsService.GetSettingValueAsync(SettingsMap.Icp);

        var logoPath = await globalSettingsService.GetSettingValueAsync(SettingsMap.ProjectLogo);
        if (!string.IsNullOrWhiteSpace(logoPath))
        {
            model.LogoUrl = storageService.RelativePathToInternetUrl(logoPath, HttpContext);
        }

        return View(model);
    }
}
