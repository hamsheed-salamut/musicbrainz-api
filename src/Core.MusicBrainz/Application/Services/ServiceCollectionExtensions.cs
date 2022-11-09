using Microsoft.Extensions.DependencyInjection;

namespace Core.MusicBrainz.Application.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArtistService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IArtistService, ArtistService>();

            return serviceCollection;
        }
    }
}
