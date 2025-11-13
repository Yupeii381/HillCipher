using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HillCipher.DataAccess.Postgres.Configurations;

public class TextConfiguration : IEntityTypeConfiguration<TextEntity>
{
    public void Configure(EntityTypeBuilder<TextEntity> builder)
    {
<<<<<<< HEAD
    builder.ToTable("Texts");
    builder.HasKey(t => t.Id);

    builder.Property(t => t.Content)
        .IsRequired()
        .HasMaxLength(1000);

    builder.HasIndex(t => t.UserId);
=======
        builder.ToTable("Texts");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(t => t.UserId)
>>>>>>> 6c008c3 (as)

    builder.HasOne(t => t.User)
        .WithMany(u => u.Texts)
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}