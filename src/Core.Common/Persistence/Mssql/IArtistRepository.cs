using Core.Common.Entities;
using Core.Common.Extensions;
using Core.Common.Models;
using System.Threading.Tasks;
using Artist = Core.Common.Entities.Artist;

namespace Core.Common.Persistence.Mssql
{
    public interface IArtistRepository
    {
        Task<PagedList<Artist>> GetAll(ArtistRequest artistRequest);
        Task<Artist> GetById(string id);
    }
}
