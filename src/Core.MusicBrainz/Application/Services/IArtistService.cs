using Core.Common.Models;
using System.Threading.Tasks;

namespace Core.MusicBrainz.Application.Services
{
    public interface IArtistService
    {
        Task<ArtistResponse> SearchArtist(string artistName, int pageNumber, int pageSize);
        Task<AlbumResponse> SearchAlbum(string artistId, int albumCount = 10);
    }
}
