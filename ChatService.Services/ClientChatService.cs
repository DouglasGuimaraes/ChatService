using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatService.Models.AbsModels.ChatConnection;
using ChatService.Models.Models;
using ChatService.Services.Interfaces;

namespace ChatService.Services
{
    public class ClientChatService : IClientChatService
    {

        #region [ FIELDS & PROPERTIES ]
        public string IpAddressNumber { get; set; }
        public StreamWriter Writer { get; set; }
        public StreamReader Reader { get; set; }
        public TcpClient TcpClient { get; set; }
        public Thread ThreadMessage { get; set; }
        public IPAddress IpAddress { get; set; }
        public bool Connected { get; set; }
        public bool UserAlreadyTaken { get; set; }

        #endregion

        #region [ CONSTRUCTOR ]

        public ClientChatService()
        {
            IpAddressNumber = "192.168.0.18";
        }

        #endregion

        #region [ METHODS ]

        #region [ PUBLIC ]

        public ChatConnectionResult Connect(ChatUser user)
        {
            ChatConnectionResult result;
            try
            {

                // IP Address object based on the IP Address string
                IpAddress = IPAddress.Parse(IpAddressNumber);

                // Initiate a new TCP connection with the server
                TcpClient = new TcpClient();
                TcpClient.Connect(IpAddressNumber, 2502);

                // Property responsible to manage the connectivity
                Connected = true;


                // Send the nickname to the server
                Writer = new StreamWriter(TcpClient.GetStream());
                Writer.WriteLine(user.Nickname);
                Writer.Flush();

                // Starts a thread to receiving messages e new conversations
                ThreadMessage = new Thread(new ThreadStart(GetMessages));
                ThreadMessage.Start();

                Thread.Sleep(2000);

                result = new ChatConnectionResult(Connected, UserAlreadyTaken);
            }
            catch (Exception ex)
            {
                result = new ChatConnectionResult(ex);
            }

            return result;
        }

        public ChatConnectionResult Disconnect()
        {
            ChatConnectionResult result;
            try
            {
                Connected = false;
                Reader.Close();
                Writer.Close();
                TcpClient.Close();

                result = new ChatConnectionResult(true);
            }
            catch (Exception ex)
            {
                result = new ChatConnectionResult(ex);
            }

            return result;
        }

        /// <summary>
        /// Send message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {   
                Writer.WriteLine(message);
                Writer.Flush();
            }
        }

        #endregion

        #region [ PRIVATE ]

        /// <summary>
        /// Get messages from the server.
        /// </summary>
        private void GetMessages()
        {
            // Receive the response from the server
            var reader = new StreamReader(TcpClient.GetStream());
            string ConResposta = reader.ReadLine();

            // If the first character is 1, the connection was done successfully
            if (ConResposta[0] == '1')
            {
                // Atualiza o formulário para informar que esta conectado
                Console.WriteLine("Connected successfully to the chat!");
                Connected = true;
            }
            else // If not so, connection was not successfully
            {
                string reason = "Not connected: ";

                // Get the reason
                reason += ConResposta.Substring(2, ConResposta.Length - 2);

                Connected = false;

                // Check if the reason was based on the "User Already Taken" to restart the process
                if(reason.Contains("This is nickname already exists in the chat"))
                    UserAlreadyTaken = true;
                
                return;
            }

            // While connected, show the messages in the Server Console
            while (Connected)
            {
                var list = reader.ReadLine();
                Console.WriteLine(list);
            }
        }

        #endregion

        #endregion
    }
}
