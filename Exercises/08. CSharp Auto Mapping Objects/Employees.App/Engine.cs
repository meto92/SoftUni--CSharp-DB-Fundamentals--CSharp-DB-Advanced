using System;

namespace Employees.App
{
    public class Engine
    {
        private CommandParser commandParser;

        public Engine(CommandParser commandParser)
        {
            this.commandParser = commandParser;
        }

        public void Run()
        {
            while (true)
            {
                string input = Console.ReadLine();

                string[] arguments = input.Split(new[] { ' ', '\t' },
                    StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    Command command = this.commandParser.ParseCommand(arguments);

                    command.Execute();
                }
                catch(ArgumentNullException ane)
                {
                    Console.WriteLine(ane.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}