using System;

using Microsoft.Extensions.DependencyInjection;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core
{
    public class Engine
    {
        private IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            this.serviceProvider
                .GetService<IDatabaseInitializerService>()
                .InitializeDatabase();

            ICommandDispatcher commandDispatcher = this.serviceProvider.GetService<ICommandDispatcher>();

            while (true)
            {
                string input = Console.ReadLine();

                try
                {
                    string result = commandDispatcher.Dispatch(input);

                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                }
            }
        }
    }
}