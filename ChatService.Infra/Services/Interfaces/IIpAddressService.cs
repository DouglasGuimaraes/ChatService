using System;
using ChatService.Models.AbsModels.IpAddressService;

namespace ChatService.Infra.Services.Interfaces
{
    public interface IIpAddressService
    {
        GetLocalIpResult GetLocalIp();
    }
}
