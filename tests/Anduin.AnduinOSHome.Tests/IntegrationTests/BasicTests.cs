using System.Diagnostics.CodeAnalysis;
using Aiursoft.CSTools.Tools;
using Anduin.AnduinOSHome.Models;
using AngleSharp.Html.Dom;
using Anduin.AnduinOSHome.Tests.Tools;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using static Aiursoft.WebTools.Extends;

namespace Anduin.AnduinOSHome.Tests.IntegrationTests;

[TestClass]
public class BasicTests
{
    private readonly string _endpointUrl;
    private readonly int _port;
    private readonly HttpClient _http;
    private IHost? _server;

    public BasicTests()
    {
        _port = Network.GetAvailablePort();
        _endpointUrl = $"http://localhost:{_port}";
        _http = new HttpClient();
    }

    [TestInitialize]
    public async Task CreateServer()
    {
        _server = await AppAsync<Startup>([], port: _port);
        await _server.StartAsync();
    }

    [TestCleanup]
    [ExcludeFromCodeCoverage]
    public async Task CleanServer()
    {
        if (_server == null) return;
        await _server.StopAsync();
        _server.Dispose();
    }

    [TestMethod]
    [DataRow("/")]
    [DataRow("/hOmE?aaaaaa=bbbbbb")]
    [DataRow("/hOmE/InDeX")]
    public async Task GetHome(string url)
    {
        var response = await _http.GetAsync(_endpointUrl + url);
        var doc = await HtmlHelpers.GetDocumentAsync(response);

        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        var p = doc.QuerySelector("p.text-lg") as IHtmlElement;
        Assert.IsTrue(p?.InnerHtml.Contains("AnduinOS is a custom Ubuntu-based"));
    }

    [TestMethod]
    [DataRow("/privacy.html")]
    public async Task GetPrivacy(string url)
    {
        var response = await _http.GetAsync(_endpointUrl + url);
        var doc = await HtmlHelpers.GetDocumentAsync(response);

        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        var h1 = doc.QuerySelector("h1.text-center") as IHtmlElement;
        Assert.IsTrue(h1?.InnerHtml.Contains("Privacy Statement"));
    }

    [TestMethod]
    [DataRow("/terms.html")]
    public async Task GetTerms(string url)
    {
        var response = await _http.GetAsync(_endpointUrl + url);
        var doc = await HtmlHelpers.GetDocumentAsync(response);

        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        var h1 = doc.QuerySelector("h1.text-center") as IHtmlElement;
        Assert.IsTrue(h1?.InnerHtml.Contains("Terms of Service"));
    }

    [TestMethod]
    public async Task GetVersions()
    {
        var response = await _http.GetAsync(_endpointUrl + "/versions.json");
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var json = await response.Content.ReadAsStringAsync();

        var versions = JsonConvert.DeserializeObject<List<VersionInfo>>(json);
        if (versions == null)
        {
            Assert.Fail();
            return;
        }
        Assert.IsTrue(versions.Count > 0);
        Assert.AreEqual(versions.First().Version, "1.0");
        Assert.IsFalse(versions.First().IsVisible);
        Assert.IsFalse(versions.First().LargeCard);
        Assert.IsFalse(versions.First().Recommended);

        var version1_3 = versions[3];
        Assert.AreEqual(version1_3.Version, "1.3");
        Assert.IsTrue(version1_3.IsVisible);
        Assert.IsTrue(version1_3.LargeCard);
        Assert.IsTrue(version1_3.Recommended);
    }

    [TestMethod]
    [DataRow("1.3")]
    [DataRow("1.2")]
    [DataRow("1.1")]
    [DataRow("1.0")]
    public async Task GetUpgrade(string branch)
    {
        var response = await _http.GetAsync(_endpointUrl + "/upgrade/" + branch);
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var json = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(json.Contains(branch));
    }

    [TestMethod]
    public async Task GetUpgradeNotFound()
    {
        var response = await _http.GetAsync(_endpointUrl + "/upgrade/1.33");
        try
        {
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Fail();
        }
        catch (HttpRequestException)
        {
        }
    }
}
