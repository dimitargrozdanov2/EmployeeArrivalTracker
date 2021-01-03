using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReportingTool.Web.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public void ConfigureDefaultRequestHeaders(Action<HttpRequestHeaders> configure)
            => configure.Invoke(httpClient.DefaultRequestHeaders);

        public Uri BaseAddress
        {
            get => httpClient.BaseAddress;
            set => httpClient.BaseAddress = value;
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return httpClient.GetAsync(requestUri);
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return httpClient.PostAsync(requestUri, content);
        }
    }
}
