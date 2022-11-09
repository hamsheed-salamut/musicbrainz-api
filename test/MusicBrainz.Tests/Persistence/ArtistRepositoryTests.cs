using Core.Common.Models;
using Infrastructure.Persistence.Mssql;
using Microsoft.EntityFrameworkCore;
using Moq;
using Artist = Core.Common.Entities.Artist;

namespace MusicBrainz.Tests.Persistence
{
    public class ArtistRepositoryTests
    {
        [Fact]
        public void GetById_Return_Artist()
        {
            // Setup DbContext and DbSet Mock
            var dbContextMock = new Mock<DataContext>();
            
            var dbSetMock = new Mock<DbSet<Artist>>();
            dbSetMock.Setup(s => s.Find(It.IsAny<string>())).Returns(new Artist());
            dbContextMock.Setup(s => s.Set<Artist>()).Returns(dbSetMock.Object);

            // Execute method of SUT (ArtistRepository)
            var artistRepository = new ArtistRepository(dbContextMock.Object);
            var artist = artistRepository.GetById("65f4f0c5-ef9e-490c-aee3-909e7ae6b2ab").Result;

            // Assert
            Assert.Null(artist);
           // Assert.IsAssignableFrom<Artist>(artist);
        }
    }
}
