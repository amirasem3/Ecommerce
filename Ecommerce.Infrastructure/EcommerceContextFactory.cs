using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Persistence;

public class EcommerceContextFactory : IDesignTimeDbContextFactory<EcommerceDbContext>
{
    public EcommerceDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<EcommerceDbContext>()
            .UseNpgsql(configuration.GetConnectionString("EcommerceDB_Post"),
                npgsqlOptions => npgsqlOptions.UseNetTopologySuite());

        return new EcommerceDbContext(builder.Options);
    }
    
    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Ecommerce.WebAPI"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange:true);

        return builder.Build();
    }
}