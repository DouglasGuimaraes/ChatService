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

        private string GetHelpText()
        {
            string helpText = string.Empty;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("### CHAT - USER GUIDE ###");
                sb.AppendLine("");
                sb.AppendLine("# SEND MESSAGE TO EVERYONE #");
                sb.AppendLine("You just need to chat your message and click in ENTER.");

                sb.AppendLine("");
                sb.AppendLine("# SEND MESSAGE TO SPECIFIC USER #");
                sb.AppendLine("You need to identify the user and start the message with /p userName.");
                sb.AppendLine("Example: /p Maria Hey Maria, how are you doing?");
                sb.AppendLine("This message will be sent only to Maria.");

                sb.AppendLine("");
                sb.AppendLine("# GET ALL USERS IN THE CHAT #");
                sb.AppendLine("You need to send the command: /users and a list of all users will appear in your chat.");
                sb.AppendLine("Example: /users");

                sb.AppendLine("");
                sb.AppendLine("You need to send the command: /exit and you will leave from the chat.");
                sb.AppendLine("Example: /exit");

                helpText = sb.ToString();

            }
            catch (Exception ex)
            {

            }

            return helpText;

        }
    }
}
