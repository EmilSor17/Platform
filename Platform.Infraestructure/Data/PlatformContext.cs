using Microsoft.EntityFrameworkCore;
using Platform.Core.Entities;
using System.Reflection;

namespace Platform.Infraestructure.Data
{
  public class PlatformContext : DbContext
  {
    public PlatformContext()
    {
    }

    public PlatformContext(DbContextOptions<PlatformContext> options)
        : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Movement> Movements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
  }
}
