using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Core.Entities;

namespace Platform.Infraestructure.Data.Configuration
{
  public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
  {
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
      builder.ToTable("Wallet");

      builder.HasKey(w => w.Id);

      builder.Property(w => w.DocumentId)
          .IsRequired()
          .HasMaxLength(20);

      builder.Property(w => w.Name)
          .IsRequired()
          .HasMaxLength(100);

      builder.Property(w => w.Balance)
          .HasColumnType("decimal(18,2)")
          .IsRequired();

      builder.Property(w => w.CreatedAt)
          .IsRequired();

      builder.HasMany(w => w.Movements)
          .WithOne(m => m.Wallet)
          .HasForeignKey(m => m.WalletId);
    }
  }

}
