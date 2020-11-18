using System;
using Microsoft.Extensions.DependencyInjection;
using ChatService.Services;
using ChatService.Services.Interfaces;

namespace ChatService.Server
{
    class Program
    {
        private static ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            serviceProvider = RegisterDependencyInjection();

            Console.Clear();
            Console.WriteLine("*** Server's starting...");

            StartServer();

        }

        /// <summary>
        /// Method responsible to add the DI in the project accessing the Dependency Injection Service
        /// </summary>
        /// <returns></returns>
        private static ServiceProvider RegisterDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();
            return DependencyInjectionService.ConfigureServices(serviceCollection);
        }

        private static void StartServer()
        {
            try
            {
                var proxy = serviceProvider.GetService<IServerChatService>();
                proxy.StartServer();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("*** Server started successfully.");
                Console.ResetColor();
                
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("*** ERROR:");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}
