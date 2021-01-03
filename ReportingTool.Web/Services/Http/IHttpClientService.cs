using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReportingTool.Web.Services.Http
{
    public interface IHttpClientService
    {
        Uri BaseAddress { get; set; }

        void ConfigureDefaultRequestHeaders(Action<HttpRequestHeaders> configure);

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }
}
