using System;
namespace ChatService.Models.Interfaces
{
    public interface IChatMessages
    {
        void GetMessages();
        void SendMessage(string message);
    }
}
