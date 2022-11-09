using RestSharp;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Transport.RestHttp
{
    public class HttpTransportService : IHttpTransportService
    {
        private readonly ILogger _logger;
        public HttpTransportService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<RestResponse> SendAsync(HttpTransportMessage message)
        {
            try
            {
                using (var restClient = new RestClient(message.Uri))
                {
                    var request = new RestRequest(message.RelativePath);
                    request.Method = Method.Get;
                    request.AddHeader("Accept", message.ContentType);

                    return await restClient.ExecuteAsync(request);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while processing REST Http request {ex}", ex.Message);
                throw;
            }
        }
    }
}
