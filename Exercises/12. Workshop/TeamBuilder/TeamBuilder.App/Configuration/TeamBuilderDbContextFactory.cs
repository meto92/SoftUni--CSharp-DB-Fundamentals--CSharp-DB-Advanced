using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using TeamBuilder.Data;

namespace TeamBuilder.App.Configuration
{
    public class TeamBuilderDbContextFactory : IDesignTimeDbContextFactory<TeamBuilderContext>
    {
        public TeamBuilderContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var builder = new DbContextOptionsBuilder<TeamBuilderContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new TeamBuilderContext(builder.Options);
        }
    }
}