using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using ChatService.Services.Interfaces;

namespace ChatService.Services
{
    public class ChatConnectionService
    {
        private readonly IServerChatService _serverChatService;

        TcpClient tcpCliente;
        // A thread que ira enviar a informação para o cliente
        private Thread thrSender;
        private StreamReader srReceptor;
        private StreamWriter swEnviador;
        private string usuarioAtual;
        private string strResposta;

        // O construtor da classe que que toma a conexão TCP
        public ChatConnectionService(TcpClient tcpCon, IServerChatService serverChatService)
        {
            tcpCliente = tcpCon;
            // A thread que aceita o cliente e espera a mensagem
            thrSender = new Thread(AcceptClient);
            // A thread chama o método AceitaCliente()
            thrSender.Start();

            _serverChatService = serverChatService;
        }

        private void CloseConnection()
        {
            // Fecha os objetos abertos
            tcpCliente.Close();
            srReceptor.Close();
            swEnviador.Close();
        }

        // Ocorre quando um novo cliente é aceito
        private void AcceptClient()
        {
            srReceptor = new System.IO.StreamReader(tcpCliente.GetStream());
            swEnviador = new System.IO.StreamWriter(tcpCliente.GetStream());

            // Lê a informação da conta do cliente
            usuarioAtual = srReceptor.ReadLine();

            // temos uma resposta do cliente
            if (usuarioAtual != "")
            {
                // Armazena o nome do usuário na hash table
                if (ServerChatService.Users.Contains(usuarioAtual) == true)
                {
                    // 0 => significa não conectado
                    swEnviador.WriteLine("0|This is nickname already exists in the chat.");
                    swEnviador.Flush();
                    CloseConnection();
                    return;
                }
                else if (usuarioAtual == "Administrator")
                {
                    // 0 => não conectado
                    swEnviador.WriteLine("0|Wow! You can't be the Administrator! hehe");
                    swEnviador.Flush();
                    CloseConnection();
                    return;
                }
                else
                {
                    // 1 => conectou com sucesso
                    swEnviador.WriteLine("1");
                    swEnviador.Flush();

                    // Inclui o usuário na hash table e inicia a escuta de suas mensagens
                    _serverChatService.AddUser(tcpCliente, usuarioAtual);
                }
            }
            else
            {
                CloseConnection();
                return;
            }
            //
            try
            {
                // Continua aguardando por uma mensagem do usuário
                while ((strResposta = srReceptor.ReadLine()) != "")
                {
                    // Se for inválido remove-o
                    if (strResposta == null)
                    {
                        _serverChatService.RemoveUser(tcpCliente);
                    }
                    else
                    {
                        // envia a mensagem para todos os outros usuários
                        _serverChatService.SendMessage(usuarioAtual, strResposta);
                    }
                }
            }
            catch
            {
                // Se houve um problema com este usuário desconecta-o
                _serverChatService.RemoveUser(tcpCliente);
            }
        }
    }
}
