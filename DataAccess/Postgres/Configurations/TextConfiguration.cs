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

        builder.Property(t => t.Plaintext)
            .IsRequired();

        builder.Property(t => t.Ciphertext)
            .IsRequired();

        builder.Property(t => t.Alphabet)
            .IsRequired();

    }
}