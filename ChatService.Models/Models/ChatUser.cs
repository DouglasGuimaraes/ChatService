using System;
using ChatService.Models.Interfaces;

namespace ChatService.Models.Models
{
    public class ChatUser : IChatUser
    {
        public string Nickname { get; set; }
    }
}
