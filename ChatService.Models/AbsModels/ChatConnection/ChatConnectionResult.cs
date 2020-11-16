using System;
namespace ChatService.Models.AbsModels.ChatConnection
{
    public class ChatConnectionResult
    {
        public bool Success { get; set; }
        public string Reason { get; set; }
        public Exception Exception { get; set; }

        public ChatConnectionResult(bool success)
        {
            this.Success = success;
        }

        public ChatConnectionResult(Exception ex)
        {
            this.Success = false;
            this.Exception = ex;
        }
    }
}
