using System;
namespace ChatService.Models.AbsModels.IpAddressService
{
    public class GetLocalIpResult
    {
        public GetLocalIpResult(string ipAddress)
        {
            Success = true;
            IpAddress = ipAddress;
            Exception = null;
        }

        public GetLocalIpResult(Exception ex)
        {
            Success = false;
            Exception = ex;
            IpAddress = null;
        }

        public bool Success { get; set; }
        public string IpAddress { get; set; }
        public Exception Exception { get; set; }
    }
}
