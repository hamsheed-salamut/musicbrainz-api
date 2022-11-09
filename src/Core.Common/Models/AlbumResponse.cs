using System.Collections.Generic;

namespace Core.Common.Models
{
    public class AlbumResponse
    {
        public List<ReleaseDto> Releases { get; set; }
        public AlbumResponse()
        {
            Releases = new List<ReleaseDto>();
        }
    }

    public class ReleaseDto
    {
        public string ReleaseId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Label { get; set; }
        public int NumberOfTracks { get; set; }
        public List<FeaturedArtist> OtherArtists { get; set; }

        public ReleaseDto()
        {
            OtherArtists = new List<FeaturedArtist>();
        }
    }

    public class FeaturedArtist
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
