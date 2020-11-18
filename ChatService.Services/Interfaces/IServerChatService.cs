using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatService.Models.AbsModels.ServerConnection;

namespace ChatService.Services.Interfaces
{
    public interface IServerChatService
    {
        string IpAddressNumber { get; set; }
        bool IsServerRunning { get; set; }

        IPAddress IpAddress { get; set; }
        TcpListener TcpListener { get; set; }
        Thread ThreadListener { get; set; }

        ServerConnectionResult AddUser(TcpClient tcpClient, string nickname);
        ServerConnectionResult RemoveUser(TcpClient tcpClient);
        ServerConnectionResult SendMessage(string source, string message);
        void StartServer();
        void StopServer();
        void KeepServer();
    }
}
