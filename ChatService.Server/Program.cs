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

            StartServer();

            Console.WriteLine("*** Server's starting...");

            Console.WriteLine("Hello World!");
        }

        private static ServiceProvider RegisterDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();
            return DependencyInjectionService.ConfigureServices(serviceCollection);
        }

        private static void StartServer()
        {
            var proxy = serviceProvider.GetService<IServerChatService>();
            proxy.StartServer();
        }
    }
}
