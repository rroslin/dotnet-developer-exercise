using Domain;
using Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new EmploymentConfiguration());
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Employment> Employments { get; set; }
}
