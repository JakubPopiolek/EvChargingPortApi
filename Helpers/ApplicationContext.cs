using EvApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EvApplicationApi.Helpers;

public class ApplicationContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public ApplicationContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public DbSet<ApplicationItem> ApplicationItems { get; set; } = null!;
}
