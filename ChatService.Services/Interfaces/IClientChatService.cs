using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatService.Models.AbsModels.ChatConnection;
using ChatService.Models.Models;

namespace ChatService.Services.Interfaces
{
    public interface IClientChatService
    {
        string IpAddressNumber { get; set; }
        bool Connected { get; set; }

        StreamWriter Writer { get; set; }
        StreamReader Reader { get; set; }
        TcpClient TcpClient { get; set; }
        Thread ThreadMessage { get; set; }
        IPAddress IpAddress { get; set; }

        ChatConnectionResult Connect(ChatUser user);
        ChatConnectionResult Disconnect(ChatUser user);
        void SendMessage(string message, string user = "");
    }
}
