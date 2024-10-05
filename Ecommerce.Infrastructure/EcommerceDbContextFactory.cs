using Ecommerce.Infrastructure.Persistence;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ecommerce.Infrastructure.Persistence
{
    public class ECommerceDbContextFactory : IDesignTimeDbContextFactory<EcommerceDbContext>
    {
        public EcommerceDbContext CreateDbContext(string[] args)
        {
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var webApiProjectPath = Path.Combine(projectPath, "Ecommerce.WebAPI");


            // Load configuration from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(webApiProjectPath)
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("EcommerceDB_Post");

            // Create DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<EcommerceDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new EcommerceDbContext(optionsBuilder.Options);
        }
    }
}
