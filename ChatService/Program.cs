using System;
using System.Linq;
using ChatService.Models.AbsModels.ChatConnection;
using ChatService.Models.AbsModels.ClientProgram;
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

                    var msgConfiguration = CheckMessageConfiguration(userMessage);

                    if(msgConfiguration.Success)
                        SendMessage(msgConfiguration.FormattedMessage);
                    else
                    {
                        Console.WriteLine("*** ERROR SENDING MESSAGE:");
                        Console.WriteLine(msgConfiguration.Exception?.Message);
                    }
                        
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

        private static void SendMessage(string message)
        {
            try
            {
                var reservatedKey = CheckReservatedKey(message);
;
                if (!reservatedKey)
                { 
                    var proxy = serviceProvider.GetService<IClientChatService>();
                    proxy.SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*** CONNECTION ERROR:");
                Console.WriteLine(ex.Message);
            }

        }

        private static ChatMessageConfiguration CheckMessageConfiguration(string message)
        {
            ChatMessageConfiguration result = null;

            try
            {
                // User Message
                var globalMessageCheck = message.Contains("/u");
                // Private MEssage
                var privateMessageCheck = message.Contains("/pu");

                // Global message

                if (!globalMessageCheck && !privateMessageCheck)
                    result = new ChatMessageConfiguration(null, message, false);

                // Global Message To User
                if(globalMessageCheck)
                {
                    var split = message.Split(" ");
                    var user = split[1];
                    var ind1 = message.IndexOf(' ');
                    var ind2 = message.IndexOf(' ', ind1 + 1);
                    var localMessage = message.Substring(ind2);
                    var privateMessage = false;
                    var formattedMessage = $"{user}|||{localMessage}";
                    result = new ChatMessageConfiguration(user, formattedMessage, privateMessage);
                }

                // Private Message to User
                if(privateMessageCheck)
                {
                    var split = message.Split(" ");
                    var user = split[1];
                    var ind1 = message.IndexOf(' ');
                    var ind2 = message.IndexOf(' ', ind1 + 1);
                    var localMessage = message.Substring(ind2);
                    var privateMessage = true;
                    var formattedMessage = $"{user}|||{localMessage}|||private";
                    result = new ChatMessageConfiguration(user, formattedMessage, privateMessage);
                }

            }
            catch (Exception ex)
            {
                result = new ChatMessageConfiguration(ex);
            }

            return result;
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
