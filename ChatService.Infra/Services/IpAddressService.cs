using System;
using System.Net;
using System.Net.Sockets;
using ChatService.Infra.Services.Interfaces;
using ChatService.Models.AbsModels.IpAddressService;

namespace ChatService.Infra.Services
{
    public class IpAddressService : IIpAddressService
    {

        public GetLocalIpResult GetLocalIp()
        {
            GetLocalIpResult result = null;

            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        result = new GetLocalIpResult(ip.ToString());
                    }
                }

                if (result == null)
                    throw new Exception("IP not found.");
            }
            catch (Exception ex)
            {
                result = new GetLocalIpResult(ex);
            }

            return result;
        }
    }
}
