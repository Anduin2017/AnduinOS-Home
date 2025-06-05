using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Primitives;

namespace Anduin.AnduinOSHome;

public class StartsWithAcceptLanguageProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var acceptLangHeader = httpContext.Request.Headers["Accept-Language"];
        if (StringValues.IsNullOrEmpty(acceptLangHeader))
        {
            return Task.FromResult<ProviderCultureResult?>(null);
        }

        var languages = acceptLangHeader
            .ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var langWithQ in languages)
        {
            var pureLang = langWithQ.Split(';', StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            if (string.IsNullOrEmpty(pureLang))
                continue;

            foreach (var result in from supported in Startup.SupportedCultures.Keys
                     where supported.StartsWith(pureLang, StringComparison.OrdinalIgnoreCase)
                     select new ProviderCultureResult(supported, supported))
            {
                return Task.FromResult(result);
            }
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }
}
