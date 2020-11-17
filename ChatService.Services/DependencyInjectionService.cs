using ChatService.Infra.Services;
using ChatService.Infra.Services.Interfaces;
using ChatService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
                   .AddSingleton<IIpAddressService, IpAddressService>()
                   .BuildServiceProvider();
        }
    }
}
