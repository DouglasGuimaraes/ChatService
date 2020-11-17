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

            Console.WriteLine("*** Server's starting...");

            StartServer();

            
        }

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

            }
            catch (Exception ex)
            {
                Console.WriteLine("*** ERROR:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
