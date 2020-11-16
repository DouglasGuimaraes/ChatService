using System;
using System.IO;
using ChatService.Models.Interfaces;

namespace ChatService.Models.Models
{
    public class ChatMessages : IChatMessages
    {
        private readonly IChatConnection _chatConnection;
        public ChatMessages(IChatConnection chatConnection)
        {
            _chatConnection = chatConnection;
        }

        public void GetMessages()
        {
            // recebe a resposta do servidor
            var reader = new StreamReader(_chatConnection.TcpClient.GetStream());
            string ConResposta = reader.ReadLine();
            // Se o primeiro caracater da resposta é 1 a conexão foi feita com sucesso
            if (ConResposta[0] == '1')
            {
                // Atualiza o formulário para informar que esta conectado
                Console.WriteLine("Connected successfully!");
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
            while (_chatConnection.Connected)
            {
                // exibe mensagems no Textbox
                var list = reader.ReadLine();
                Console.WriteLine(list);
            }
        }

        public void SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {   //escreve a mensagem da caixa de texto
                _chatConnection.Writer.WriteLine(message);
                _chatConnection.Writer.Flush();
            }
        }
    }
}
