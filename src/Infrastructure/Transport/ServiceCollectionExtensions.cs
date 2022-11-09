using Core.Common.AppConfiguration;
using Infrastructure.Transport.RestHttp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Transport
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpTransport(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var musicBrainzOptions = new MusicBrainzOptions();
            configuration.GetSection("MusicBrainzOptions").Bind(musicBrainzOptions);
            serviceCollection.AddSingleton(musicBrainzOptions);

            serviceCollection.AddSingleton(typeof(IHttpTransportService), typeof(HttpTransportService));

            return serviceCollection;

        }
    }
}
