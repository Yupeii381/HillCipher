using HillCipher.DataAccess.Postgres.Configurations;
using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace HillCipher.DataAccess.Postgres;

public class CipherDbContext(DbContextOptions<CipherDbContext> options) : DbContext(options)
{

    public DbSet<TextEntity> Texts { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<RequestHistory> RequestHistories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TextConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RequestHistoryConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}