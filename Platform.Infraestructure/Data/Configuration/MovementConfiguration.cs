using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Core.Entities;
using Platform.Core.Enums;

namespace Platform.Infraestructure.Data.Configuration
{
  public class MovementConfiguration : IEntityTypeConfiguration<Movement>
  {
    public void Configure(EntityTypeBuilder<Movement> builder)
    {
      builder.ToTable("Movement");

      builder.HasKey(m => m.Id);

      builder.Property(m => m.Amount)
          .HasColumnType("decimal(18,2)")
          .IsRequired();

      builder.Property(m => m.CreatedAt)
          .IsRequired();

      builder.Property(m => m.Type)
          .IsRequired()
          .HasConversion(
              x => x.ToString(),
              x => (MovementType)Enum.Parse(typeof(MovementType), x)
          );
    }
  }

}
