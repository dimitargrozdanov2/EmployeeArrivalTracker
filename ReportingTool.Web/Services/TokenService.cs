using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReportingTool.Data.Exceptions;
using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;
using ReportingTool.Web.Services.Http;
using ReportingTool.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly IServiceTokenRepository serviceTokenRepository;
        private readonly IHttpClientService httpClientService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenService(IServiceTokenRepository serviceTokenRepository, IHttpClientService httpClientService, IHttpContextAccessor httpContextAccessor)
        {
            this.serviceTokenRepository = serviceTokenRepository;
            this.httpClientService = httpClientService;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceToken> GetServiceToken(DateTime dayOfArrival)
        {
            var token = new ServiceToken();
            var queryString = $"?date={dayOfArrival:yyyy-MM-dd}&callback={TokenConstants.CallbackUrl}";
            var request = $"{TokenConstants.WebSiteUrl}{TokenConstants.SubscriptionUrl}{queryString}";
            httpClientService.ConfigureDefaultRequestHeaders(h => h.Add("Accept-Client", TokenConstants.ClientHeaderValue));
            httpClientService.BaseAddress = new Uri(TokenConstants.WebSiteUrl);
            var responseMessage = await httpClientService.GetAsync(request);
            if (responseMessage.IsSuccessStatusCode)
            {
                var tokenInfo = await responseMessage.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<ServiceToken>(tokenInfo);
            }
            return token;
        }

        public void TryGetTokenFromSession(string token)
        {
            if (!this.httpContextAccessor.HttpContext.Session.TryGetValue("ServiceToken", out byte[] value))
            {
                this.httpContextAccessor.HttpContext.Session.SetString("ServiceToken", token);
                this.httpContextAccessor.HttpContext.Session.SetString("ServiceTokenExpiry", token);

            }
            else
            {
                // Get The Token.
                string result = Encoding.UTF8.GetString(value);
                this.httpContextAccessor.HttpContext.Session.SetString("ServiceToken", result);

                // TODO: Validate Here Or In The Middleware.
            }
        }

        public async Task SavesTokenAsync(ServiceToken token) => await this.serviceTokenRepository.AddAsync(token);

        public async Task<IEnumerable<Arrival>> CollectArrivals(HttpRequest request)
        {
            using var stream = new StreamReader(request.Body);
            string jsonData = await stream.ReadToEndAsync();
            return JsonConvert.DeserializeObject<List<Arrival>>(jsonData);
        }

        public bool TokenHasExpired(string tokenExpireTime)
        {
            var tokenExpireDateTime = DateTimeOffset.Parse(tokenExpireTime).UtcDateTime;
            var tokenExpireComparison = DateTimeOffset.Compare(DateTime.UtcNow, tokenExpireDateTime);
            if (tokenExpireComparison > 0)
            {
                throw new TokenExpiryException($"{tokenExpireDateTime}");
            }
            return false;
        }

        public async Task<bool> TokenAlreadyExistsAsync(HttpRequest request)
        {
            var serviceToken = await ReadTokenAsync(request);

            return serviceToken != null;
        }

        public async Task<ServiceToken> ReadTokenAsync(HttpRequest request)
        {
            if (request.Headers.ContainsKey(TokenConstants.ServiceTokenHeader))
            {
                var tokenValue = request.Headers[TokenConstants.ServiceTokenHeader].ToString();
                //var tokenModel = await this.serviceTokenRepository.GetSingleAsync(t => t.Token == tokenValue);

                var y = httpContextAccessor.HttpContext.Session.Keys.ToList();

                this.TryGetTokenFromSession(tokenValue);

                //if (!TokenHasExpired(tokenModel.Expires))
                //{
                //    return tokenModel;
                //}
            }
            return null;
        }
    }
}
