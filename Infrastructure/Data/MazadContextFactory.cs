using EcommerceIti.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EcommerceIti.Infrastructure.Data;

public class MazadContextFactory : IDesignTimeDbContextFactory<MazadContext>
{
    public MazadContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<MazadContext>();
        var connectionString = configuration.GetConnectionString("MazadContext")
            ?? "Server=.;Database=EcommerceIti;Trusted_Connection=True;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(connectionString);

        return new MazadContext(optionsBuilder.Options);
    }
}
