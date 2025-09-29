//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//namespace HillCipher.DataAccess.Postgres
//{
//    public class CipherDbContextFactory : IDesignTimeDbContextFactory<CipherDbContext>
//    {
//        public CipherDbContext CreateDbContext(string[] args)
//        {
//            // Укажем, где искать конфиги
//            var configuration = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory()) 
//                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
//                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
//                .AddEnvironmentVariables()
//                .Build();

//            var connectionString = configuration.GetConnectionString("CipherDbContext");

//            var optionsBuilder = new DbContextOptionsBuilder<CipherDbContext>();
//            optionsBuilder.UseNpgsql(connectionString);

//            return new CipherDbContext(optionsBuilder.Options);
//        }
//    }
//}