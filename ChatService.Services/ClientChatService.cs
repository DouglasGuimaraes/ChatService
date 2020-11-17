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
        public string IpAddressNumber { get; set; }
        public StreamWriter Writer { get; set; }
        public StreamReader Reader { get; set; }
        public TcpClient TcpClient { get; set; }
        public Thread ThreadMessage { get; set; }
        public IPAddress IpAddress { get; set; }
        public bool Connected { get; set; }

        public ClientChatService()
        {
            IpAddressNumber = "192.168.0.18";
        }

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
                ThreadMessage = new Thread(new ThreadStart(GetMessages));
                ThreadMessage.Start();

                result = new ChatConnectionResult(true);
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

        public void SendMessage(string message, string user = "")
        {
            if (!string.IsNullOrEmpty(message))
            {   //escreve a mensagem da caixa de texto
                Writer.WriteLine(message);
                Writer.Flush();
            }
        }

        private void GetMessages()
        {
            // recebe a resposta do servidor
            var reader = new StreamReader(TcpClient.GetStream());
            string ConResposta = reader.ReadLine();
            // Se o primeiro caracater da resposta é 1 a conexão foi feita com sucesso
            if (ConResposta[0] == '1')
            {
                // Atualiza o formulário para informar que esta conectado
                Console.WriteLine("Connected successfully to the chat!");
            }
            else // Se o primeiro caractere não for 1 a conexão falhou
            {
                string reason = "Not connected: ";
                // Extrai o motivo da mensagem resposta. O motivo começa no 3o caractere
                reason += ConResposta.Substring(2, ConResposta.Length - 2);
                // Atualiza o formulário como o motivo da falha na conexão
                Console.WriteLine(reason);
                // Sai do método
                return;
            }

            // Enquanto estiver conectado le as linhas que estão chegando do servidor
            while (Connected)
            {
                // exibe mensagems no Textbox
                var list = reader.ReadLine();
                Console.WriteLine(list);
            }
        }

    }
}
