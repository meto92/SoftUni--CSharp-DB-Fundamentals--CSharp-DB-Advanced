using System;
using System.Data.SqlClient;

using BusTicketsSystem.Client.Core.Interfaces;
using BusTicketsSystem.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace BusTicketsSystem.Client.Core
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

            ICommandInterpreter commandInterpreter = serviceProvider.GetService<ICommandInterpreter>();

            while (true)
            {
                string[] arguments = Console.ReadLine()
                    .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    string result = commandInterpreter.InterpretCommand(arguments);

                    Console.WriteLine(result);
                }
                catch (Exception ex) 
                    when (ex is SqlException ||
                        ex is ArgumentException || 
                        ex is InvalidOperationException)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}