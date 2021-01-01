using Newtonsoft.Json;
using ReportingTool.Services.Contracts;
using ReportingTool.Services.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportingTool.Services
{
    public class ServiceTokenService : IServiceTokenService
    {
        public async Task<ServiceToken> GetServiceToken(string websiteUrl, DateTime dayOfArrival, string callbackUrl) 
        {
            var token = new ServiceToken();
            using HttpClient client = new HttpClient();
            var request = $"{TokenConstants.SubscriptionUrl}?date={dayOfArrival.ToString("yyyy-MM-dd")}&callback={callbackUrl}";
            client.BaseAddress = new Uri(websiteUrl);
            client.DefaultRequestHeaders.Add("Accept-Client", TokenConstants.ClientHeaderValue);
            var responseMessage = await client.GetAsync(request);
            if (responseMessage.IsSuccessStatusCode)
            {
                var tokenInfo = await responseMessage.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<ServiceToken>(tokenInfo);
            }
            return token;
        }
    }
}
