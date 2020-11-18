using ChatService.Services;
using Microsoft.Extensions.DependencyInjection;
using ChatService.Services;
using ChatService.Services.Interfaces;
using ChatService.Models.Constants.ServerChatService;
using Xunit;
using System.Net;
using System.Net.Sockets;

namespace ChatService.Tests
{
    public class ServerChatServiceTests
    {
        private static ServiceProvider serviceProvider;
        private readonly IServerChatService serverChatService;

        public ServerChatServiceTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceProvider = DependencyInjectionService.ConfigureServicesTransient(serviceCollection);

            serverChatService = serviceProvider.GetService<IServerChatService>();
            serverChatService.StartServer();
        }

        [Fact]
        public void ServerIsRunning_SuccessTest()
        {
            TcpClient tcpClient = new TcpClient();
            string ip = ServerChatServiceConstants.MANUAL_IP;
            tcpClient.Connect(ip, ServerChatServiceConstants.SERVER_PORT);
            Assert.True(tcpClient.Connected);
        }

        [Theory]
        [InlineData("Douglas")]
        public void AddUser_SuccessTest(string nickname)
        {

            TcpClient tcpClient = new TcpClient();
            string ip = ServerChatServiceConstants.MANUAL_IP;
            tcpClient.Connect(ip, ServerChatServiceConstants.SERVER_PORT);

            var user = serverChatService.AddUser(tcpClient, nickname);

            Assert.True(user.Success);
        }

        [Theory]
        [InlineData("Douglas")]
        public void RemoveUser_SuccessTest(string nickname)
        {

            TcpClient tcpClient = new TcpClient();
            string ip = ServerChatServiceConstants.MANUAL_IP;
            tcpClient.Connect(ip, ServerChatServiceConstants.SERVER_PORT);

            var addUser = serverChatService.AddUser(tcpClient, nickname);

            var removeUser = serverChatService.RemoveUser(tcpClient);

            Assert.True(removeUser.Success);
        }

        [Theory]
        [InlineData("Douglas", "Send Global Message.")]
        public void SendGlobalMessage_SuccessTest(string nickname, string message)
        {
            TcpClient tcpClient = new TcpClient();
            string ip = ServerChatServiceConstants.MANUAL_IP;
            tcpClient.Connect(ip, ServerChatServiceConstants.SERVER_PORT);

            var addUser = serverChatService.AddUser(tcpClient, nickname);

            var removeUser = serverChatService.SendMessage(nickname, message);

            Assert.True(removeUser.Success);
        }

        [Theory]
        [InlineData("Douglas", "Send Global Message To One User.")]
        public void SendGlobalMessageToOneUser_SuccessTest(string nickname, string message)
        {

            TcpClient tcpClient = new TcpClient();
            string ip = ServerChatServiceConstants.MANUAL_IP;
            tcpClient.Connect(ip, ServerChatServiceConstants.SERVER_PORT);

            var addUser = serverChatService.AddUser(tcpClient, nickname);

            var removeUser = serverChatService.SendMessage(nickname, $"{nickname}|||{message}");

            Assert.True(removeUser.Success);
        }

        [Theory]
        [InlineData("Douglas", "Send Private Message.")]
        public void SendPrivateMessage_SuccessTest(string nickname, string message)
        {

            TcpClient tcpClient = new TcpClient();
            string ip = ServerChatServiceConstants.MANUAL_IP;
            tcpClient.Connect(ip, ServerChatServiceConstants.SERVER_PORT);

            var addUser = serverChatService.AddUser(tcpClient, nickname);

            var removeUser = serverChatService.SendMessage(nickname, $"{nickname}|||{message}|||private");

            Assert.True(removeUser.Success);
        }

    }
}
