using System;
using ChatService.Services;

namespace ChatService
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterDependencyInjection(args);

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        private static void RegisterDependencyInjection(string[] args)
        {
            DependencyInjectionService.CreateHostBuilder(args);
        }
    }
}
