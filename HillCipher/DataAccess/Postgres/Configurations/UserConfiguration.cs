using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HillCipher.DataAccess.Postgres.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
            builder.ToTable("Users");
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Username)
        .IsRequired()
        .HasMaxLength(50)
        .IsUnicode(false);

    builder.Property(x => x.PasswordHash)
        .IsRequired()
        .HasMaxLength(100); 

    builder.HasIndex(x => x.Username).IsUnique();

    builder.HasMany(x => x.Texts)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}