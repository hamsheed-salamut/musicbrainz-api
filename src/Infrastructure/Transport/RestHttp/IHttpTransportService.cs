using RestSharp;
using System.Threading.Tasks;

namespace Infrastructure.Transport.RestHttp
{
    public interface IHttpTransportService
    {
        Task<RestResponse> SendAsync(HttpTransportMessage message);
    }
}
