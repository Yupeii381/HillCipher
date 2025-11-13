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

        builder.Property(x => x.Action).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ActionName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ErrorMessage).HasMaxLength(500);
        builder.Property(x => x.InputText).HasMaxLength(500);
        builder.Property(x => x.ResultText).HasMaxLength(500);
        builder.Property(x => x.Timestamp).HasDefaultValue("NOW()");
    
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Timestamp);
    }
}