using System;
using ChatService.Services;
using ChatService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ChatService.Tests
{
    public class ClientChatServiceTests
    {

        private static ServiceProvider serviceProvider;

        public ClientChatServiceTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceProvider = DependencyInjectionService.ConfigureServicesTransient(serviceCollection);
        }

        //[Theory]
        //[InlineData("192.168.0.18")]
        //public void ServerConnect_SuccessTest(string ipAddress)
        //{
        //    var service = serviceProvider.GetService<IClientChatService>();

        //    service.IpAddressNumber = ipAddress;

        //    var connect = service.Connect(new Models.Models.ChatUser { Nickname = Guid.NewGuid().ToString() });

        //    Assert.True(connect.Success);
        //}

        //[Fact]
        //public void ServerConnect_FailTest()
        //{
        //    var service = serviceProvider.GetService<IClientChatService>();

        //    // Force error
        //    service.IpAddressNumber = "";

        //    var connect = service.Connect(new Models.Models.ChatUser { Nickname = Guid.NewGuid().ToString() });

        //    service.Disconnect();

        //    Assert.False(connect.Success);
        //}

        //[Fact]
        //public void ServerConnect_SameNicknameValidation_SuccessTest()
        //{
        //    var service = serviceProvider.GetService<IClientChatService>();

        //    var nickname = Guid.NewGuid().ToString();

        //    var connect = service.Connect(new Models.Models.ChatUser { Nickname = nickname });
        //    var connect2 = service.Connect(new Models.Models.ChatUser { Nickname = nickname });

        //    service.Disconnect();

        //    Assert.False(connect2.Success);
        //}

        [Fact]
        public void SendMessage_SuccessTest()
        {
            var service = serviceProvider.GetService<IClientChatService>();
            var connect = service.Connect(new Models.Models.ChatUser { Nickname = Guid.NewGuid().ToString() });
            var message = service.SendMessage("Test message.");

            Assert.True(message.Success);
        }

        //[Fact]
        //public void SendMessage_FailTest()
        //{
        //    var service = serviceProvider.GetService<IClientChatService>();
        //    var connect = service.Connect(new Models.Models.ChatUser { Nickname = Guid.NewGuid().ToString() });

        //    // Force disconnect and messge will be not sent
        //    service.Disconnect();

        //    var message = service.SendMessage("Test message.");

        //    Assert.False(message.Success);
        //}

        //[Fact]
        //public void SendMessage_EmptyMessage_SuccessTest()
        //{
        //    var service = serviceProvider.GetService<IClientChatService>();
        //    var connect = service.Connect(new Models.Models.ChatUser { Nickname = Guid.NewGuid().ToString() });

        //    var message = service.SendMessage("");

        //    service.Disconnect();

        //    Assert.False(message.Success);
        //}
    }
}
