using Microsoft.AspNetCore.Http;
using ReportingTool.Data.Models;
using ReportingTool.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportingTool.Web.Services
{
    public interface IServiceTokenService
    {
        Task<ServiceToken> GetServiceToken(string websiteUrl, DateTime dayOfArrival, string callbackUrl);
        Task<ServiceToken> ReadTokenAsync(HttpRequest request);
        Task<bool> TokenAlreadyExistsAsync(HttpRequest request);
        bool TokenHasExpired(string tokenExpireTime);
        Task<IEnumerable<Arrival>> CollectArrivals(HttpRequest request);
        Task SavesTokenAsync(ServiceToken token);
    }
}