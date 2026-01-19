using Aiursoft.DbTools;
using Aiursoft.DbTools.InMemory;
using Anduin.AnduinOSHome.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Anduin.AnduinOSHome.InMemory;

public class InMemorySupportedDb : SupportedDatabaseType<AnduinOSHomeDbContext>
{
    public override string DbType => "InMemory";

    public override IServiceCollection RegisterFunction(IServiceCollection services, string connectionString)
    {
        return services.AddAiurInMemoryDb<InMemoryContext>();
    }

    public override AnduinOSHomeDbContext ContextResolver(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<InMemoryContext>();
    }
}
