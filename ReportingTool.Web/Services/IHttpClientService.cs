using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReportingTool.Web.Services
{
    public interface IHttpClientService
    {
        Uri BaseAddress { get; set; }

        void ConfigureDefaultRequestHeaders(Action<HttpRequestHeaders> configure);

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }
}
