using System;
using System.Text;
using ChatService.Services.Interfaces;

namespace ChatService.Services
{
    public class UserGuideService : IUserGuideService
    {
        public void Help()
        {
            Console.WriteLine(GetHelpText());
        }

        /// <summary>
        /// Get the HELP text that will be showed in the Console
        /// </summary>
        /// <returns></returns>
        private string GetHelpText()
        {
            string helpText = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("### CHAT - USER GUIDE ###");
            sb.AppendLine("");
            sb.AppendLine("# SEND MESSAGE TO EVERYONE #");
            sb.AppendLine("You just need to chat your message and click in ENTER.");

            sb.AppendLine("");
            sb.AppendLine("# SEND GLOBAL MESSAGE TO SPECIFIC USER #");
            sb.AppendLine("You need to identify the user and start the message with /u nickname and write your message after.");
            sb.AppendLine("Example: /u Maria Hey Maria, how are you doing?");
            sb.AppendLine("This message will be sent publically to Maria.");

            sb.AppendLine("");
            sb.AppendLine("# SEND GLOBAL MESSAGE TO SPECIFIC USER #");
            sb.AppendLine("You need to identify the user and start the message with /pu nickname write your message after.");
            sb.AppendLine("Example: /pu Maria Hey Maria, how are you doing?");
            sb.AppendLine("This message will be sent privatelly to Maria.");

            sb.AppendLine("");
            sb.AppendLine("You need to send the command: /exit and you will leave from the chat.");
            sb.AppendLine("Example: /exit");

            helpText = sb.ToString();

            return helpText;

        }
    }
}
