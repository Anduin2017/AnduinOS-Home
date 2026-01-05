using Microsoft.AspNetCore.Mvc;
using Aiursoft.Canon;
using Aiursoft.WebTools.Attributes;
using Anduin.AnduinOSHome.Models;
using Microsoft.Extensions.Options;

namespace Anduin.AnduinOSHome.Controllers;

[LimitPerMin]
public class UpgradeController(
    CacheService cacheService,
    IOptions<List<VersionInfo>> versions,
    HttpClient httpClient,
    IOptions<List<string>> endpoints,
    RetryEngine retryEngine,
    ILogger<UpgradeController> logger) : ControllerBase
{
    [HttpGet("upgrade/{branch}")]
    public async Task<IActionResult> Get(string branch)
    {
        var isAvailableBranch = versions.Value.Any(t => t.Version == branch);
        if (!isAvailableBranch)
        {
            return NotFound();
        }

        // Set client timeout to 15 seconds.
        httpClient.Timeout = TimeSpan.FromSeconds(15);

        var upgradeContent = await cacheService.RunWithCache($"upgrade-content-{branch}", () =>
                retryEngine.RunWithRetry(async _ =>
                {
                    // Try each endpoint in order until one succeeds
                    Exception? lastException = null;

                    foreach (var endpoint in endpoints.Value)
                    {
                        try
                        {
                            var url = endpoint.Replace("{Branch}", branch);
                            logger.LogInformation("Attempting to download upgrade script from {Endpoint}", url);
                            var response = await httpClient.GetAsync(url);
                            response.EnsureSuccessStatusCode();
                            logger.LogInformation("Successfully downloaded upgrade script from {Endpoint}", url);
                            return await response.Content.ReadAsStringAsync();
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            logger.LogWarning(ex, "Failed to download upgrade script from {Endpoint}. Trying next endpoint.", endpoint);
                        }
                    }

                    // If all endpoints failed, throw the last exception
                    throw lastException ?? new InvalidOperationException("No endpoints available");
                }, attempts: 3)
            , cachedMinutes: _ => TimeSpan.FromMinutes(10));
        // content type is bash.
        return Content(upgradeContent, "application/x-sh");
    }
}
