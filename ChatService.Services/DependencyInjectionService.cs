using ChatService.Infra.Services;
using ChatService.Infra.Services.Interfaces;
using ChatService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Services
{
    public class DependencyInjectionService
    {
        /// <summary>
        /// Configure all services in the DI
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
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
