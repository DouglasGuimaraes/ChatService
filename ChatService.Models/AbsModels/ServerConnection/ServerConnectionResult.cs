using System;
namespace ChatService.Models.AbsModels.ServerConnection
{
    public class ServerConnectionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public ServerConnectionResult(bool success, string message = "")
        {
            this.Success = success;
            this.Message = message;
        }

        public ServerConnectionResult(Exception ex)
        {
            this.Success = false;
            this.Exception = ex;
        }
    }
}
