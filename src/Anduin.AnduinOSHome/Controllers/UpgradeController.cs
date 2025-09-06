using Microsoft.AspNetCore.Mvc;
using Aiursoft.Canon;
using Anduin.AnduinOSHome.Models;
using Microsoft.Extensions.Options;

namespace Anduin.AnduinOSHome.Controllers;

public class UpgradeController(
    CacheService cacheService,
    IOptions<List<VersionInfo>> versions,
    HttpClient httpClient,
    IOptions<List<string>> endpoints,
    RetryEngine retryEngine) : ControllerBase
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
            retryEngine.RunWithRetry(async i => // i starts with 1.
            {
                // For i = 1,2, try endpoint[0]
                // For i = 3,4, try endpoint[1]
                var endpointIndex = (i - 1) / 2;
                if (endpointIndex >= endpoints.Value.Count)
                    endpointIndex = endpoints.Value.Count - 1;

                var endpointToTry = endpoints.Value[endpointIndex];
                var response = await httpClient.GetAsync(endpointToTry.Replace("{Branch}", branch));
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }, attempts: 6)
        , cachedMinutes: _ => TimeSpan.FromMinutes(10));
        return Content(upgradeContent, "application/json");
    }
}
