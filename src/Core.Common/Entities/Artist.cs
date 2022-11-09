using System;

namespace Core.Common.Entities
{
    public class Artist
    {
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string Country { get; set; }
        public string Aliases { get; set; }
    }
}
