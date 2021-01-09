using Microsoft.AspNetCore.Http;
using ReportingTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportingTool.Web.Services
{
    public interface ITokenService
    {
        Task<ServiceToken> GetServiceToken(DateTime dayOfArrival);

        Task<ServiceToken> ReadTokenAsync(HttpRequest request);

        Task<bool> TokenAlreadyExistsAsync(HttpRequest request);

        bool TokenHasExpired(string tokenExpireTime);

        Task<IEnumerable<Arrival>> CollectArrivals(HttpRequest request);

        Task SavesTokenAsync(ServiceToken token);

        void TryGetTokenFromSession(string token);
    }
}