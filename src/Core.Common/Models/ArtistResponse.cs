using System.Collections.Generic;

namespace Core.Common.Models
{
    public class ArtistResponse
    {
        public List<ArtistDto> Results { get; set; }
        public int NumberOfSearchResults { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int NumberOfPages { get; set; }

        public ArtistResponse()
        {
            Results = new List<ArtistDto>();
        }
    }

    public class ArtistDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public List<string> Alias { get; set; }

        public ArtistDto()
        {
            Alias = new List<string>();
        }
    }
}
