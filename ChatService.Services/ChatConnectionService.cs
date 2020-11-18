using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using ChatService.Services.Interfaces;

namespace ChatService.Services
{
    public class ChatConnectionService
    {
        #region [ FIELDS & PROPERTIES ]

        private readonly IServerChatService _serverChatService;

        TcpClient TcpClient;
        private Thread ThreadSender;
        private StreamReader Reader;
        private StreamWriter Writer;
        private string UserInformation;
        private string Response;

        #endregion

        #region [ CONSTRUCTOR ]

        /// <summary>
        /// The class Constructor that receives the new TCP Connection, accepting the new client and waiting
        /// for the messages.
        /// </summary>
        /// <param name="tcpCon"></param>
        /// <param name="serverChatService"></param>
        public ChatConnectionService(TcpClient tcpCon, IServerChatService serverChatService)
        {
            TcpClient = tcpCon;
            
            ThreadSender = new Thread(AcceptClient);

            ThreadSender.Start();

            _serverChatService = serverChatService;
        }

        #endregion

        #region [ METHODS ]

        #region [ PRIVATE ]

        /// <summary>
        /// Accept the Client inserting him in the Server Hash Tables and starts
        /// to listening user messages.
        /// </summary>
        private void AcceptClient()
        {
            Reader = new System.IO.StreamReader(TcpClient.GetStream());
            Writer = new System.IO.StreamWriter(TcpClient.GetStream());

            // Read the user information
            UserInformation = Reader.ReadLine();

            // If the information exists
            if (UserInformation != "")
            {
                // Check if the user already exists in the current connection
                if (ServerChatService.Users.Contains(UserInformation) == true)
                {
                    // 0: not connected
                    Writer.WriteLine("0|This is nickname already exists in the chat.");
                    Writer.Flush();
                    CloseConnection();
                    return;
                }
                else if (UserInformation == "Administrator")
                {
                    // 0: not connected
                    Writer.WriteLine("0|Wow! You can't be the Administrator! hehe");
                    Writer.Flush();
                    CloseConnection();
                    return;
                }
                else
                {
                    // 1: Connected successfully
                    Writer.WriteLine("1");
                    Writer.Flush();

                    // Add the user in the HashTable and starts the message listener
                    _serverChatService.AddUser(TcpClient, UserInformation);
                }
            }
            else
            {
                CloseConnection();
                return;
            }
            
            try
            {

                // Still waiting for new user messages
                while ((Response = Reader.ReadLine()) != "")
                {
                    // If invalid, remove the user
                    if (Response == null)
                    {
                        _serverChatService.RemoveUser(TcpClient);
                    }
                    else
                    {
                        // If OK, send the message to the Server treatment
                        _serverChatService.SendMessage(UserInformation, Response);
                    }
                }
            }
            catch
            {
                // If any problem with the user, user is disconnected
                _serverChatService.RemoveUser(TcpClient);
            }
        }

        private void CloseConnection()
        {
            TcpClient.Close();
            Reader.Close();
            Writer.Close();
        }

        #endregion

        #endregion
    }
}
