using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatService.Services.Interfaces;

namespace ChatService.Services
{
    public class ServerChatService : IServerChatService
    {
        private const int _MaxUsers = 15;

        public static Hashtable Users { get; set; }
        public static Hashtable Connections { get; set; }

        public string IpAddressNumber { get; set; }
        public bool IsServerRunning { get; set; }
        public IPAddress IpAddress { get; set; }
        public TcpListener TcpListener { get; set; }
        public Thread ThreadListener { get; set; }

        public ServerChatService()
        {
            Users = new Hashtable(_MaxUsers);
            Connections = new Hashtable(_MaxUsers);
            IpAddressNumber = "192.168.0.18";
            IpAddress = IPAddress.Parse(IpAddressNumber);
        }

        public void AddUser(TcpClient tcpClient, string nickname)
        {
            // Primeiro inclui o nome e conexão associada para ambas as hash tables
            Users.Add(nickname, tcpClient);
            Connections.Add(tcpClient, nickname);

            // Informa a nova conexão para todos os usuário e para o formulário do servidor
            UpdateServerInformation($"{Connections[tcpClient]} entrou.");
            
        }

        public void RemoveUser(TcpClient tcpClient)
        {
            // Se o usuário existir
            if (Connections[tcpClient] != null)
            {
                // Primeiro mostra a informação e informa os outros usuários sobre a conexão
                UpdateServerInformation($"{Connections[tcpClient]} saiu.");

                // Removeo usuário da hash table
                Users.Remove(Connections[tcpClient]);
                Connections.Remove(tcpClient);
            }
        }

        public void SendMessage(string source, string message, string user = "")
        {
            StreamWriter swSenderSender;

            string fullMessage = $"{source} disse: {message}.";
            // Primeiro exibe a mensagem na aplicação
            UpdateServerInformation(fullMessage);

            // Cria um array de clientes TCPs do tamanho do numero de clientes existentes
            TcpClient[] tcpClientes = new TcpClient[Users.Count];

            // Copia os objetos TcpClient no array
            Users.Values.CopyTo(tcpClientes, 0);
            // Percorre a lista de clientes TCP
            for (int i = 0; i < tcpClientes.Length; i++)
            {
                // Tenta enviar uma mensagem para cada cliente
                try
                {
                    // Se a mensagem estiver em branco ou a conexão for nula sai...
                    if (message.Trim() == "" || tcpClientes[i] == null)
                    {
                        continue;
                    }

                    // Envia a mensagem para o usuário atual no laço
                    swSenderSender = new StreamWriter(tcpClientes[i].GetStream());
                    swSenderSender.WriteLine(fullMessage);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
                catch // Se houver um problema , o usuário não existe , então remove-o
                {
                    RemoveUser(tcpClientes[i]);
                }
            }
        }

        public void StartServer()
        {
            // Pega o IP do primeiro dispostivo da rede
            IPAddress ipaLocal = IpAddress;

            // Cria um objeto TCP listener usando o IP do servidor e porta definidas
            TcpListener = new TcpListener(ipaLocal, 2502);

            // Inicia o TCP listener e escuta as conexões
            TcpListener.Start();

            // O laço While verifica se o servidor esta rodando antes de checar as conexões
            IsServerRunning = true;

            // Inicia uma nova tread que hospeda o listener
            ThreadListener = new Thread(KeepServer);
            ThreadListener.Start();
        }

        public void KeepServer()
        {
            // Enquanto o servidor estiver rodando
            while (IsServerRunning == true)
            {
                // Aceita uma conexão pendente
                var tcpClient = TcpListener.AcceptTcpClient();
                // Cria uma nova instância da conexão
                Conexao newConnection = new Conexao(tcpClient, this);
            }
        }

        private void UpdateServerInformation(string message)
        {
            Console.WriteLine(message);
        }
    }
}
