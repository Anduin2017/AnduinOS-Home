using Anduin.AnduinOSHome.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anduin.AnduinOSHome.InMemory;

public class InMemoryContext(DbContextOptions<InMemoryContext> options) : TemplateDbContext(options)
{
    public override Task MigrateAsync(CancellationToken cancellationToken)
    {
        return Database.EnsureCreatedAsync(cancellationToken);
    }

    public override Task<bool> CanConnectAsync()
    {
        return Task.FromResult(true);
    }
}
