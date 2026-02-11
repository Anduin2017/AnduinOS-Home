using Aiursoft.Scanner.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Anduin.AnduinOSHome.Services.FileStorage;

public class StorageRootPathProvider(IConfiguration configuration) : ISingletonDependency
{
    public string GetStorageRootPath()
    {
        var path = configuration["Storage:Path"] ?? "/tmp/data";
        return path;
    }
}
