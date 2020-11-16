using System;
using System.Linq;
using ChatService.Models.AbsModels.ChatConnection;
using ChatService.Models.Interfaces;
using ChatService.Services;
using ChatService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService
{
    class Program
    {
        #region [ FIELDS & PROPERTIES ]

        private static ServiceProvider serviceProvider;

        #endregion

        #region [ MAIN ]

        static void Main(string[] args)
        {

            // Service Provider
            serviceProvider = RegisterDependencyInjection();

            // Welcome and User Nickname
            var nickname = SetWelcome();

            // Server connection
            var connection = GetConnection(nickname);

            if (connection.Success)
            {
                Console.WriteLine("*** SEND YOUR MESSAGE:");
                while (true)
                {
                    var userMessage = Console.ReadLine();

                    var directMessage = CheckDirectMessage(userMessage);

                    SendMessage(userMessage, directMessage);
                }
            }
            else
            {
                Console.WriteLine("*** CONNECTION ERROR:");
                Console.WriteLine(connection.Exception.Message);
            }

            Console.ReadKey();
        }

        #endregion

        #region [ PRIVATE METHODS ]

        private static ServiceProvider RegisterDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();
            return DependencyInjectionService.ConfigureServices(serviceCollection);
        }

        private static string SetWelcome()
        {
            Console.WriteLine("*** Welcome to our chat service!");
            Console.WriteLine("*** Please set your Nickname:");
            return Console.ReadLine();
        }

        private static ChatConnectionResult GetConnection(string nickname)
        {
            var proxy = serviceProvider.GetService<IClientChatService>();
            var connection = proxy.Connect(new Models.Models.ChatUser { Nickname = nickname });
            return connection;
        }

        private static void SendMessage(string message, string user = "")
        {
            var proxy = serviceProvider.GetService<IClientChatService>();
            proxy.SendMessage(message, user);

        }

        private static string CheckDirectMessage(string message)
        {
            var split = message.Split("/p");

            if (split.Count() > 1)
                return split[0];
            else
                return "";
        }

        #endregion
    }
}
