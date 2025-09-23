using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HillCipher.DataAccess.Postgres.Configurations;

public class TextConfiguration : IEntityTypeConfiguration<TextEntity>
{
    public void Configure(EntityTypeBuilder<TextEntity> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .HasOne(u => u.User)
            .WithMany(t => t.Texts)
            .HasForeignKey(t => t.UserId);
    }
}
