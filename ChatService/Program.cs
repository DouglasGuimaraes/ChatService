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

            bool userAlreadyTaken = false;
            if(args.Count() == 1)
                userAlreadyTaken = true;

            // Welcome and User Nickname
            var nickname = SetWelcome(userAlreadyTaken);

            // Server connection
            var connection = GetConnection(nickname.ToUpper());

            if(!connection.Success && connection.UserAlreadyTaken)
            {
                Program.Main(new string[] { "UserAlreadyTaken" });
            }
            else if (connection.Success)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("*** Welcome to the chat! ***");
                Console.WriteLine("*** Use the /help command to list all the commands available to you.");
                Console.ResetColor();
                
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

        /// <summary>
        /// Method responsible to add the DI in the project accessing the Dependency Injection Service
        /// </summary>
        /// <returns></returns>
        private static ServiceProvider RegisterDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();
            return DependencyInjectionService.ConfigureServices(serviceCollection);
        }

        private static string SetWelcome(bool userAlreadyTaken = false)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (!userAlreadyTaken)
            {
                Console.Clear();
                Console.WriteLine("*** Welcome to our chat service!");
                Console.WriteLine("*** Please set your nickname:");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("*** Welcome to our chat service!");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("*** Nickname is already taken.");
                Console.WriteLine("*** Please set other nickname:");
            }

            Console.ResetColor();
            return Console.ReadLine();
        }

        /// <summary>
        /// Create the connection with the server.
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        private static ChatConnectionResult GetConnection(string nickname)
        {
            var proxy = serviceProvider.GetService<IClientChatService>();
            var connection = proxy.Connect(new Models.Models.ChatUser { Nickname = nickname });
            return connection;
        }

        /// <summary>
        /// Send message to the server.
        /// </summary>
        /// <param name="message"></param>
        private static void SendMessage(string message)
        {
            try
            {
                if(!string.IsNullOrEmpty(message))
                { 
                    var reservatedKey = CheckReservatedKey(message);
    
                    if (!reservatedKey)
                    { 
                        var proxy = serviceProvider.GetService<IClientChatService>();
                        proxy.SendMessage(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("*** CONNECTION ERROR:");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

        }

        /// <summary>
        /// Check if the message was sent global or privatelly.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static ChatMessageConfiguration CheckMessageConfiguration(string message)
        {
            ChatMessageConfiguration result = null;

            try
            {
                // User Message
                var globalMessageCheck = message.StartsWith("/u");

                // Private MEssage
                var privateMessageCheck = message.StartsWith("/pu");

                // Global message
                if (!globalMessageCheck && !privateMessageCheck)
                    result = new ChatMessageConfiguration(null, message, false);

                // Global Message To User
                if(globalMessageCheck)
                {
                    var split = message.Split(" ");
                    var user = split[1].ToUpper();
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
                    var user = split[1].ToUpper();
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

        /// <summary>
        /// Check if the message contains some reserved key of the application.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool CheckReservatedKey(string message)
        {
            var helper = message.ToUpper().Contains(UserGuideConstants.HELP.ToUpper());
            if (helper)
            {
                var proxy = serviceProvider.GetService<IUserGuideService>();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("");
                Console.WriteLine(" > HELP command:");
                proxy.Help();

                Console.WriteLine("");
                Console.WriteLine(" > END of HELP command:");
                Console.WriteLine("");
                Console.ResetColor();
            }

            var exit = message.ToUpper().Contains(UserGuideConstants.EXIT.ToUpper());
            if (exit)
            {
                var proxy = serviceProvider.GetService<IClientChatService>();
                proxy.Disconnect();
                Console.WriteLine("*** Thank you for using the chat. Bye!");
                Environment.Exit(0);

            }

            return helper || exit;
        }

        #endregion
    }
}
