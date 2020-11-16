using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChatService.Models.Interfaces;
using ChatService.Models.Models;

namespace ChatService.Services
{
    public class DependencyInjectionService
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices
            ((_, services) =>
                    services
                        .AddSingleton<IChatConnection, ChatConnection>()
                        .AddSingleton<IChatMessages, ChatMessages>()
                        .AddSingleton<IChatUser, ChatUser>()
            );
    }
}
