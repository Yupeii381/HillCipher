using HillCipher.DataAccess.Postgres.Configurations;
using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace HillCipher.DataAccess.Postgres;

public class CipherDbContext(DbContextOptions<CipherDbContext> options) : DbContext(options)
{

    public DbSet<TextEntity> Texts { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TextConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}