using Anduin.AnduinOSHome.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anduin.AnduinOSHome.MySql;

public class MySqlContext(DbContextOptions<MySqlContext> options) : TemplateDbContext(options);
