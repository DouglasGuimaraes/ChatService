using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatService.Infra.Services.Interfaces;
using ChatService.Models.Constants.ServerChatService;
using ChatService.Services.Interfaces;

namespace ChatService.Services
{
    public class ServerChatService : IServerChatService
    {
        #region [ FIELDS & PROPERTIES ]

        private readonly IIpAddressService _ipAddressService;

        private const int _MaxUsers = 15;

        public static Hashtable Users { get; set; }
        public static Hashtable Connections { get; set; }

        public string IpAddressNumber { get; set; }
        public bool IsServerRunning { get; set; }
        public IPAddress IpAddress { get; set; }
        public TcpListener TcpListener { get; set; }
        public Thread ThreadListener { get; set; }

        #endregion

        #region [ CONSTRUCTOR ]

        public ServerChatService(IIpAddressService ipAddressService)
        {
            _ipAddressService = ipAddressService;

            // Hash table with the max users
            Users = new Hashtable(ServerChatServiceConstants.MAX_USERS);

            // Has table with the max connections
            Connections = new Hashtable(ServerChatServiceConstants.MAX_USERS);

            // Get local IP
            var getIpAddress = _ipAddressService.GetLocalIp();

            // If GetLocalIp was not sucessfully, set the IP manually
            IpAddressNumber = getIpAddress.Success ? getIpAddress.IpAddress : "192.168.0.18";
            IpAddress =  IPAddress.Parse(IpAddressNumber);
        }

        #endregion

        #region [ METHODS ]

        #region [ PUBLIC ]

        /// <summary>
        /// Add user in the Hash Tables and inform the new connection to all users.
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="nickname"></param>
        public void AddUser(TcpClient tcpClient, string nickname)
        {
            Users.Add(nickname, tcpClient);
            Connections.Add(tcpClient, nickname);

            string newConnectionText = $"{Connections[tcpClient]} has joined.";
            UpdateServerAndUsers(newConnectionText);

        }

        /// <summary>
        /// Remove user from the Hash Tables and inform all users about it
        /// </summary>
        /// <param name="tcpClient"></param>
        public void RemoveUser(TcpClient tcpClient)
        {
            // If user already exists
            if (Connections[tcpClient] != null)
            {
                string userLefttext = $"{Connections[tcpClient]} left.";
                UpdateServerAndUsers(userLefttext);

                Users.Remove(Connections[tcpClient]);
                Connections.Remove(tcpClient);
            }
        }

        /// <summary>
        /// Send message based on the configuration (public or privately)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public void SendMessage(string source, string message)
        {
            string fullMessage = string.Empty;

            // Split the messages using |||
            // Count = 1 => Public Message
            // Count = 2 => Public Message to One User
            // Count = 3 => Private Message to Specific User
            var messageContent = message.Split("|||");
            var mesageContentCount = messageContent.Count();

            // Public message
            if (mesageContentCount == 1)
            {
                fullMessage = $"{source} says to all: {message}";
                UpdateServerInformation(fullMessage);
                SendPublicMessage(fullMessage);
            }
            // Public message to one user
            else if (mesageContentCount == 2)
            {
                string publicUser = messageContent[0];
                string publicMessage = messageContent[1];
                fullMessage = $"{source} says to {publicUser}: {publicMessage}";

                var userExists = CheckIfUserExists(publicUser);
                if (userExists)
                {
                    UpdateServerInformation(fullMessage);
                    SendPublicMessage(fullMessage);
                }
                else
                {
                    SendServerPrivateMessage(ServerChatServiceConstants.USER_NOT_FOUND, source);
                }
                
            }
            // Private message to specific user
            else if (mesageContentCount == 3)
            {
                string privateUser = messageContent[0];
                string privateMessage = messageContent[1];
                fullMessage = $"{source} says to {privateUser} (privately): {privateMessage}";

                var userExists = CheckIfUserExists(privateUser);
                if (userExists)
                {
                    UpdateServerInformation(fullMessage);
                    SendPrivateMessage(fullMessage, source, messageContent[0]);
                }
                else
                {
                    SendServerPrivateMessage(ServerChatServiceConstants.USER_NOT_FOUND, source);
                }

                
            }

        }

        /// <summary>
        /// Start the TCP Listener
        /// </summary>
        public void StartServer()
        {
            IPAddress ipaLocal = IpAddress;

            // Create a TCP Listener object using the IP and one specific port
            TcpListener = new TcpListener(ipaLocal, 2502);

            // Start the TCP Listener
            TcpListener.Start();

            IsServerRunning = true;

            // Starta a new thread to Keep the Server running and accepting hte new connections
            ThreadListener = new Thread(KeepServer);
            ThreadListener.Start();
        }

        public void KeepServer()
        {
            while (IsServerRunning == true)
            {
                // Accept a new connection
                var tcpClient = TcpListener.AcceptTcpClient();

                // Create a new instance of connection
                var newConnection = new ChatConnectionService(tcpClient, this);
            }
        }

        #endregion

        #region [ PRIVATE ]

        /// <summary>
        /// Send the message to all users in the Hash Table
        /// </summary>
        /// <param name="message"></param>
        private void SendPublicMessage(string message)
        {
            StreamWriter SenderWriter;

            // Creates a new array with the correct size (based on the Hash Table)
            TcpClient[] tcpClientes = new TcpClient[Users.Count];

            // Copy all the objects from the Hash to the array
            Users.Values.CopyTo(tcpClientes, 0);

            // For each element in the array, try to send the message
            for (int i = 0; i < tcpClientes.Length; i++)
            {
                try
                {
                    if (message.Trim() == "" || tcpClientes[i] == null)
                    {
                        continue;
                    }

                    // Send the message to the user
                    SenderWriter = new StreamWriter(tcpClientes[i].GetStream());
                    SenderWriter.WriteLine(message);
                    SenderWriter.Flush();
                    SenderWriter = null;
                }
                catch // If any problem, probably the user does not exists, so remove him
                {
                    RemoveUser(tcpClientes[i]);
                }
            }
        }

        /// <summary>
        /// Send Server message to Source user.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sourceUser"></param>
        private void SendServerPrivateMessage(string message, string sourceUser)
        {
            StreamWriter SenderWriter;

            var findSourceUser = Users[sourceUser];

            if (findSourceUser != null && findSourceUser is TcpClient)
            {
                var tcpClient = findSourceUser as TcpClient;
                try
                {
                    if (message.Trim() == "")
                    {
                        return;
                    }

                    // Send the message to the source user that has sent
                    SenderWriter = new StreamWriter(tcpClient.GetStream());
                    SenderWriter.WriteLine(message);
                    SenderWriter.Flush();
                    SenderWriter = null;
                }
                catch (Exception ex)
                {
                    RemoveUser(tcpClient);
                }

            }

        }


        /// <summary>
        /// Send private message to specific user, based on the message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sourceUser"></param>
        /// <param name="destinationUser"></param>
        private void SendPrivateMessage(string message, string sourceUser, string destinationUser)
        {
            StreamWriter SenderWriter;

            var findDestinationUser = Users[destinationUser];

            if (findDestinationUser != null && findDestinationUser is TcpClient)
            {
                var tcpClient = findDestinationUser as TcpClient;
                try
                {

                    if (message.Trim() == "")
                    {
                        return;
                    }

                    // Send the message to the specific user
                    SenderWriter = new StreamWriter(tcpClient.GetStream());
                    SenderWriter.WriteLine(message);
                    SenderWriter.Flush();
                    SenderWriter = null;
                }
                catch (Exception ex)
                {
                    RemoveUser(tcpClient);
                }

            }

            var findSourceUser = Users[sourceUser];

            if (findSourceUser != null && findSourceUser is TcpClient)
            {
                var tcpClient = findSourceUser as TcpClient;
                try
                {
                    if (message.Trim() == "")
                    {
                        return;
                    }

                    // Send the message to the source user that has sent
                    SenderWriter = new StreamWriter(tcpClient.GetStream());
                    SenderWriter.WriteLine(message);
                    SenderWriter.Flush();
                    SenderWriter = null;
                }
                catch (Exception ex)
                {
                    RemoveUser(tcpClient);
                }

            }

        }

        /// <summary>
        /// Update Server Console
        /// </summary>
        /// <param name="message"></param>
        private void UpdateServerInformation(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Update Server Console and All users
        /// </summary>
        /// <param name="message"></param>
        private void UpdateServerAndUsers(string message)
        {
            UpdateServerInformation(message);
            SendPublicMessage(message);
        }

        /// <summary>
        /// Check if user exists in the User Hash table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool CheckIfUserExists(string user)
        {
            var hashUser = Users[user];
            if (hashUser == null)
                return false;
            else
                return true;
        }

        #endregion

        #endregion
    }
}
