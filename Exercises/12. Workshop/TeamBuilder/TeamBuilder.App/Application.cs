using System;
using System.Globalization;
using System.IO;
using System.Threading;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TeamBuilder.App.Configuration;
using TeamBuilder.App.Core;
using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.Data;
using TeamBuilder.Services;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App
{
    public class Application
    {
        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            serviceCollection.AddDbContext<TeamBuilderContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies());

            serviceCollection.AddTransient<IDatabaseInitializerService, DatabaseInitializerService>();

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<TeamBuilderProfile>());

            serviceCollection.AddTransient<ICommandDispatcher, CommandDispatcher>();

            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IEventService, EventService>();
            serviceCollection.AddTransient<ITeamService, TeamService>();
            serviceCollection.AddTransient<IInvitationService, InvitationService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            IServiceProvider serviceProvider = ConfigureServices();

            Engine engine = new Engine(serviceProvider);

            engine.Run();
        }
    }
}