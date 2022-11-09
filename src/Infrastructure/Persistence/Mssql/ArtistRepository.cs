using Core.Common.Extensions;
using Core.Common.Models;
using Core.Common.Persistence.Mssql;
using System.Linq;
using System.Threading.Tasks;
using Artist = Core.Common.Entities.Artist;

namespace Infrastructure.Persistence.Mssql
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly DataContext _context;

        public ArtistRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Artist>> GetAll(ArtistRequest artistRequest)
        {
            return PagedList<Artist>.ToPagedList(FindAll().Where(x => x.ArtistName.Contains(artistRequest.ArtistName)).OrderBy(x => x.ArtistName), artistRequest.PageNumber, artistRequest.PageSize);
        }

        public async Task<Artist> GetById(string id)
        {
            return await _context.Set<Artist>().FindAsync(id);
        }

        public IQueryable<Artist> FindAll()
        {
            return _context.Set<Artist>();
        }
    }
}
