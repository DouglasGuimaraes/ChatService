using System;
namespace ChatService.Models.Interfaces
{
    public interface IChatUser
    {
        public string Nickname { get; set; }
        public string Hash { get; set; }
    }
}
