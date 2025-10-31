using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HillCipher.DataAccess.Postgres.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Username)
            .IsRequired();

        builder
            .Property(x => x.PasswordHash)
            .IsRequired();

        builder
            .Property(x => x.TokenVersion)
            .IsRequired();

        builder
            .HasMany(x => x.Texts)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}