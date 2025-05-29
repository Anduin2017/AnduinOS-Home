using Aiursoft.CSTools.Tools;
using AngleSharp.Html.Dom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anduin.AnduinOSHome.Tests.Tools;
using Microsoft.Extensions.Hosting;
using static Aiursoft.WebTools.Extends;
using Aiursoft.AiurObserver.DefaultConsumers;
using Aiursoft.AiurObserver.WebSocket;

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
}
