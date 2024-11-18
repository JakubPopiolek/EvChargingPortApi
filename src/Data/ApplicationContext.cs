using EvApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EvApplicationApi.Helpers;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ApplicationItem>()
            .HasMany(e => e.Files)
            .WithOne(e => e.ApplicationItem)
            .HasForeignKey(e => e.ApplicationReferenceNumber)
            .IsRequired(true);

        modelBuilder
            .Entity<ApplicationItem>()
            .HasOne(e => e.Address)
            .WithOne(e => e.ApplicationItem)
            .HasForeignKey<ApplicationItem>(e => e.AddressId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }

    public DbSet<ApplicationItem> ApplicationItems { get; set; }

    public DbSet<UploadedFile> UploadedFiles { get; set; }
}
