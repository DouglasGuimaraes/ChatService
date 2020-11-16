using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatService.Models.AbsModels.ChatConnection;
using ChatService.Models.Models;

namespace ChatService.Models.Interfaces
{
    public interface IChatConnection
    {


        string IpAddressNumber { get; set; }
        StreamWriter Writer { get; set; }
        StreamReader Reader { get; set; }
        TcpClient TcpClient { get; set; }

        Thread ThreadMessage { get; set; }
        IPAddress IpAddress { get; set; }

        bool Connected { get; set; }

        ChatConnectionResult Connect(ChatUser user);
        ChatConnectionResult Disconnect(ChatUser user);
    }
}
