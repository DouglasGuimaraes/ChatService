using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatService.Services.Interfaces
{
    public interface IServerChatService
    {
        string IpAddressNumber { get; set; }
        bool IsServerRunning { get; set; }

        IPAddress IpAddress { get; set; }
        TcpListener TcpListener { get; set; }
        Thread ThreadListener { get; set; }

        void AddUser(TcpClient tcpClient, string nickname);
        void RemoveUser(TcpClient tcpClient);
        void SendMessage(string source, string message);
        void StartServer();
        void KeepServer();
    }
}
