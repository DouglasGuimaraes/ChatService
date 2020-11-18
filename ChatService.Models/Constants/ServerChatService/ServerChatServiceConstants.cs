using System;
namespace ChatService.Models.Constants.ServerChatService
{
    public class ServerChatServiceConstants
    {
        /// <summary>
        /// Number of max simultaneous users in the chat.
        /// </summary>
        public const int MAX_USERS = 15;

        /// <summary>
        /// Server message when the user not exists for public and private messages.
        /// </summary>
        public const string USER_NOT_FOUND = "SERVER MESSAGE: User not found.";

        /// <summary>
        /// Server port for the TCP connection.
        /// </summary>
        public const int SERVER_PORT = 1991;

        /// <summary>
        /// IP of the computer (in case of some error getting IP)
        /// </summary>
        public const string MANUAL_IP = "192.168.0.18";
    }
}
