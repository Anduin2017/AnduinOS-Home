using System.Diagnostics.CodeAnalysis;
using Anduin.AnduinOSHome.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anduin.AnduinOSHome.MySql;

[ExcludeFromCodeCoverage]

public class MySqlContext(DbContextOptions<MySqlContext> options) : AnduinOSHomeDbContext(options);
