using System;
using System.Linq;
using ChatService.Models.AbsModels.ChatConnection;
using ChatService.Models.Constants.UserGuide;
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
                Console.WriteLine("*** User the command /help to list all the commands available to you.");
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
            Console.WriteLine("*** Please set your nickname:");
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
            try
            {
                var reservatedKey = CheckReservatedKey(message);

                if(!reservatedKey)
                { 
                    var proxy = serviceProvider.GetService<IClientChatService>();
                    proxy.SendMessage(message, user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*** CONNECTION ERROR:");
                Console.WriteLine(ex.Message);
            }

        }

        private static string CheckDirectMessage(string message)
        {
            var split = message.Split("/p");

            if (split.Count() > 1)
                return split[0];
            else
                return "";
        }

        private static bool CheckReservatedKey(string message)
        {
            var helper = message.ToUpper().Contains(UserGuideConstants.HELP.ToUpper());
            if (helper)
            {
                var proxy = serviceProvider.GetService<IUserGuideService>();
                Console.WriteLine("");
                Console.WriteLine(" > HELP command:");
                proxy.Help();

                Console.WriteLine("");
                Console.WriteLine(" > END of HELP command:");
                Console.WriteLine(" > Type any key to back to the chat.");
                Console.ReadKey();
            }

            var exit = message.ToUpper().Contains(UserGuideConstants.EXIT.ToUpper());
            if (exit)
            {
                var proxy = serviceProvider.GetService<IClientChatService>();
                proxy.Disconnect();
            }

            return helper || exit;
        }

        #endregion
    }
}
