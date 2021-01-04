﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReportingTool.Data.Exceptions;
using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;
using ReportingTool.Web.Services.Http;
using ReportingTool.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ReportingTool.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly IServiceTokenRepository serviceTokenRepository;
        private readonly IHttpClientService httpClientService;

        public TokenService(IServiceTokenRepository serviceTokenRepository, IHttpClientService httpClientService)
        {
            this.serviceTokenRepository = serviceTokenRepository;
            this.httpClientService = httpClientService;
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
                var tokenModel = await this.serviceTokenRepository.GetSingleAsync(t => t.Token == tokenValue);
                if (!TokenHasExpired(tokenModel.Expires))
                {
                    return tokenModel;
                }
            }
            return null;
        }
    }
}
