using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HillCipher.DataAccess.Postgres.Configurations;

public class TextConfiguration : IEntityTypeConfiguration<TextEntity>
{
    public void Configure(EntityTypeBuilder<TextEntity> builder)
    {
        builder.ToTable("Texts");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Content)
            .IsRequired()
            .HasMaxLength(600);

        builder.HasOne(t => t.User)
            .WithMany(u => u.Texts)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}