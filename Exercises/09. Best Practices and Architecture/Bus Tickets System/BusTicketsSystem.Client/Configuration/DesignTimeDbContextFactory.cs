using BusTicketsSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BusTicketsSystem.Client.Configuration
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BusTicketsSystemContext>
    {
        public BusTicketsSystemContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<BusTicketsSystemContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new BusTicketsSystemContext(builder.Options);
        }
    }
}