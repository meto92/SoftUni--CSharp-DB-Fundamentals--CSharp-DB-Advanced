using System;
using System.Globalization;
using System.IO;
using System.Threading;

using AutoMapper;

using BusTicketsSystem.Client.Core;
using BusTicketsSystem.Client.Core.Interfaces;
using BusTicketsSystem.Data;
using BusTicketsSystem.Services;
using BusTicketsSystem.Services.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusTicketsSystem.Client
{
    public class Startup
    {
        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            serviceCollection.AddDbContext<BusTicketsSystemContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies());

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<BusTicketsSystemProfile>());

            serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>();
            serviceCollection.AddTransient<ICommandParser, CommandParser>();
            serviceCollection.AddTransient<IDatabaseInitializerService, DatabaseInitializerService>();

            serviceCollection.AddTransient<IBusStationService, BusStationService>();
            serviceCollection.AddTransient<IBusCompanyService, BusCompanyService>();
            serviceCollection.AddTransient<ICustomerService, CustomerService>();
            serviceCollection.AddTransient<IReviewService, ReviewService>();
            serviceCollection.AddTransient<ITicketService, TicketService>();
            serviceCollection.AddTransient<ITripService, TripService>();

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