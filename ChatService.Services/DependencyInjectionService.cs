using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChatService.Models.Interfaces;
using ChatService.Models.Models;
using ChatService.Services.Interfaces;

namespace ChatService.Services
{
    public class DependencyInjectionService
    {
        public static ServiceProvider ConfigureServices(IServiceCollection services)
        {
            return
                services
                   .AddSingleton<IClientChatService, ClientChatService>()
                   .AddSingleton<IServerChatService, ServerChatService>()
                   .AddSingleton<IUserGuideService, UserGuideService>()
                   .BuildServiceProvider();
        }
    }
}
