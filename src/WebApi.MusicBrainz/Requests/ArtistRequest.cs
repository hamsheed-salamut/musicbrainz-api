namespace WebApi.MusicBrainz.Requests
{
    public class ArtistRequest
    {
        public string ArtistName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
