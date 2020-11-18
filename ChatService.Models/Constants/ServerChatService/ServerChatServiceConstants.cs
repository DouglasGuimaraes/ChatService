using System;
namespace ChatService.Models.Constants.ServerChatService
{
    public class ServerChatServiceConstants
    {
        /// <summary>
        /// Number of max simultaneous users in the chat.
        /// </summary>
        public static int MAX_USERS = 15;

        /// <summary>
        /// Server message when the user not exists for public and private messages.
        /// </summary>
        public static string USER_NOT_FOUND = "SERVER MESSAGE: User not found.";
    }
}
