using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HillCipher.DataAccess.Postgres.Configurations;

public class RequestHistoryConfiguration : IEntityTypeConfiguration<RequestHistory>
{
    public void Configure(EntityTypeBuilder<RequestHistory> builder)
    {
        builder.ToTable("RequestHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Action)
            .IsRequired();

        builder.Property(x => x.InputText)
            .HasMaxLength(200);

        builder.Property(x => x.ResultText)
            .HasMaxLength(200);

        builder.HasOne(x => x.User)
            .WithMany(r => r.RequestHistories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}