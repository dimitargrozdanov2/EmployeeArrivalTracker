using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Services.Contracts
{
    public interface IServiceTokenService
    {
        Task<ServiceToken> GetServiceToken(string websiteUrl, DateTime dayOfArrival, string callbackUrl);
    }
}
