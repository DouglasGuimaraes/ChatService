using System;
namespace ChatService.Models.AbsModels.ClientProgram
{
    public class ChatMessageConfiguration
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public string User { get; set; }
        public bool PrivateMessage { get; set; }
        public string FormattedMessage { get; set; }

        public ChatMessageConfiguration(string user, string formattedMessage, bool privateMessage)
        {
            Success = true;
            User = user;
            FormattedMessage = formattedMessage;
            PrivateMessage = privateMessage;
            Exception = null;
        }

        public ChatMessageConfiguration(Exception ex)
        {
            Success = false;
            Exception = ex;
        }

    }
}
