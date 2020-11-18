using System;
namespace ChatService.Models.AbsModels.ChatConnection
{
    public class ChatConnectionResult
    {
        public bool Success { get; set; }
        public bool UserAlreadyTaken { get; set; }
        public string Reason { get; set; }
        public Exception Exception { get; set; }

        public ChatConnectionResult(bool success, bool userAlreadyTaken = false)
        {
            this.Success = success;
            this.UserAlreadyTaken = userAlreadyTaken;
        }

        public ChatConnectionResult(Exception ex)
        {
            this.Success = false;
            this.Exception = ex;
        }
    }
}
