using System.Diagnostics.CodeAnalysis;
using static Aiursoft.WebTools.Extends;

namespace Anduin.AnduinOSHome;

[ExcludeFromCodeCoverage]
public abstract class Program
{
    public static async Task Main(string[] args)
    {
        var app = await AppAsync<Startup>(args);
        await app.RunAsync();
    }
}
