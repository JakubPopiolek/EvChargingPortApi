using EvApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EvApplicationApi.Helpers;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationItem>().HasOne(applicationItem => applicationItem.Address);
    }

    public DbSet<ApplicationItem> ApplicationItems { get; set; }
}
