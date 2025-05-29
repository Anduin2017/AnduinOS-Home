using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Aiursoft.CSTools.Tools;
using Anduin.AnduinOSHome.Models;
using AngleSharp.Html.Dom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anduin.AnduinOSHome.Tests.Tools;
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
        Assert.IsTrue(p?.InnerHtml.Contains("AnduinOS is a custom Ubuntu-based Linux distribution that aims to"));
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

        var versions = JsonSerializer.Deserialize<List<VersionInfo>>(json);
        Assert.IsTrue(versions?.Count > 0);
        Assert.AreEqual(versions.First().Version, "1.0");
        Assert.IsFalse(versions.First().IsVisible);
        Assert.IsFalse(versions.First().LargeCard);
        Assert.IsFalse(versions.First().Recommended);

        Assert.AreEqual(versions.Last().Version, "1.3");
        Assert.IsTrue(versions.Last().IsVisible);
        Assert.IsTrue(versions.Last().LargeCard);
        Assert.IsTrue(versions.Last().Recommended);
    }
}
