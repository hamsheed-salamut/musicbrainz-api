using Core.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Mssql
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) :base(options)
        {
        }

        public DataContext()
        {

        }

        public DbSet<Artist> Artist { get; set; }
    }
}
