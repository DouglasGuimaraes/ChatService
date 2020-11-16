using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatService.Models.AbsModels.ChatConnection;
using ChatService.Models.Interfaces;

namespace ChatService.Models.Models
{
    public class ChatConnection : IChatConnection
    {

        private readonly IChatMessages _chatMessages;

        
        public ChatConnection(IChatMessages chatMessages)
        {
            _chatMessages = chatMessages;
        }

        public string IpAddressNumber { get; set; }
        public StreamWriter Writer { get; set; }
        public StreamReader Reader { get; set; }
        public TcpClient TcpClient { get; set; }
        public Thread ThreadMessage { get; set; }
        public IPAddress IpAddress { get; set; }
        public bool Connected { get; set; }

        public ChatConnectionResult Connect(ChatUser user)
        {
            ChatConnectionResult result;
            try
            {

                // Trata o endereço IP informado em um objeto IPAdress
                IpAddress = IPAddress.Parse(IpAddressNumber);

                // Inicia uma nova conexão TCP com o servidor chat
                TcpClient = new TcpClient();
                TcpClient.Connect(IpAddressNumber, 2502);

                // AJuda a verificar se estamos conectados ou não
                Connected = true;


                // Envia o nome do usuário ao servidor
                Writer = new StreamWriter(TcpClient.GetStream());
                Writer.WriteLine(user.Nickname);
                Writer.Flush();

                //Inicia a thread para receber mensagens e nova comunicação
                ThreadMessage = new Thread(new ThreadStart(_chatMessages.GetMessages));
                ThreadMessage.Start();

                result = new ChatConnectionResult(true);
            }
            catch (Exception ex)
            {
                result = new ChatConnectionResult(ex);
            }

            return result;
        }

        public ChatConnectionResult Disconnect(ChatUser user)
        {
            ChatConnectionResult result;
            try
            {
                Connected = false;
                result = new ChatConnectionResult(true);
            }
            catch (Exception ex)
            {
                result = new ChatConnectionResult(ex);
            }

            return result;
        }

    }
}
