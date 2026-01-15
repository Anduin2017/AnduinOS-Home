using System.Diagnostics.CodeAnalysis;
using Anduin.AnduinOSHome.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anduin.AnduinOSHome.Sqlite;

[ExcludeFromCodeCoverage]

public class SqliteContext(DbContextOptions<SqliteContext> options) : TemplateDbContext(options)
{
    public override Task<bool> CanConnectAsync()
    {
        return Task.FromResult(true);
    }
}
