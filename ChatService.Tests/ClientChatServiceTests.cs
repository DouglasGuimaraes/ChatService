using System;
using System.Threading;
using System.Reflection;
using ChatService.Services;
using ChatService.Services.Interfaces;
using ChatService.Models.Constants.ServerChatService;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
namespace ChatService.Tests
{
    [Collection("ChatService")]
    public class ClientChatServiceTests
    {

        private static ServiceProvider serviceProvider;
        private readonly IServerChatService serverChatService;

        public ClientChatServiceTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceProvider = DependencyInjectionService.ConfigureServicesTransient(serviceCollection);

            serverChatService = serviceProvider.GetService<IServerChatService>();
            serverChatService.StartServer();
        }

        #region [ SERVER CONNECTION ]

        [Theory]
        [InlineData(ServerChatServiceConstants.MANUAL_IP)]
        public void ServerConnect_SuccessTest(string ipAddress)
        {
            var clientService = serviceProvider.GetService<IClientChatService>();

            clientService.IpAddressNumber = ipAddress;

            var nickname = MethodBase.GetCurrentMethod().Name;

            var connect = clientService.Connect(new Models.Models.ChatUser { Nickname = nickname });

            Assert.True(connect.Success);
        }

        [Fact]
        public void ServerConnect_FailTest()
        {
            var clientService = serviceProvider.GetService<IClientChatService>();

            // Force error
            clientService.IpAddressNumber = "";

            var nickname = MethodBase.GetCurrentMethod().Name;

            var connect = clientService.Connect(new Models.Models.ChatUser { Nickname = nickname });

            Assert.False(connect.Success);
        }

        #endregion

        #region [ MESSAGES ]

        [Fact]
        public void SendMessage_SuccessTest()
        {
            var clientService = serviceProvider.GetService<IClientChatService>();

            var nickname = MethodBase.GetCurrentMethod().Name;

            var connect = clientService.Connect(new Models.Models.ChatUser { Nickname = nickname });
            var message = clientService.SendMessage("Test message.");

            Assert.True(message.Success);
        }

        [Fact]
        public void SendMessage_GlobalUser_SuccessTest()
        {
            var clientService = serviceProvider.GetService<IClientChatService>();

            var nickname = MethodBase.GetCurrentMethod().Name;

            var connect = clientService.Connect(new Models.Models.ChatUser { Nickname = nickname });
            var message = clientService.SendMessage($"{nickname}|||Test Message Global User.");

            Assert.True(message.Success);
        }

        [Fact]
        public void SendMessage_PrivateUser_SuccessTest()
        {
            var clientService = serviceProvider.GetService<IClientChatService>();

            var nickname = MethodBase.GetCurrentMethod().Name;

            var connect = clientService.Connect(new Models.Models.ChatUser { Nickname = nickname });
            var message = clientService.SendMessage($"{nickname}|||Test Message Private User.|||private");

            Assert.True(message.Success);
        }

        [Fact]
        public void SendMessage_EmptyMessage_SuccessTest()
        {
            var clientService = serviceProvider.GetService<IClientChatService>();

            var nickname = MethodBase.GetCurrentMethod().Name;

            var connect = clientService.Connect(new Models.Models.ChatUser { Nickname = nickname });

            var message = clientService.SendMessage("");

            Assert.False(message.Success);
        }

        #endregion
    }
}
