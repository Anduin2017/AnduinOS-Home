using System.Net;

namespace Anduin.AnduinOSHome.Tests.IntegrationTests;

[TestClass]
public class CultureControllerTests : TestBase
{
    [TestMethod]
    public async Task SetCulture()
    {
        var url = "/Culture/Set?culture=en&returnUrl=/";
        var response = await Http.GetAsync(url);
        
        // Assert
        Assert.AreEqual(HttpStatusCode.Found, response.StatusCode);
    }
}
