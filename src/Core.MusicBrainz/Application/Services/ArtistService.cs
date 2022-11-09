using Core.Common.AppConfiguration;
using Core.Common.Models;
using Core.Common.Persistence.Mssql;
using Infrastructure.Transport;
using Infrastructure.Transport.RestHttp;
using Newtonsoft.Json;
using Serilog;
using SerilogTimings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.MusicBrainz.Application.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ILogger _logger;
        private readonly IArtistRepository _artistRepository;
        private readonly IHttpTransportService _httpTransportService;
        private readonly MusicBrainzOptions _musicBrainzOptions;

        public ArtistService(
            ILogger logger,
            IArtistRepository artistRepository,
            IHttpTransportService httpTransportService,
            MusicBrainzOptions musicBrainzOptions)
        {
            _logger = logger;
            _artistRepository = artistRepository;
            _httpTransportService = httpTransportService;
            _musicBrainzOptions = musicBrainzOptions;
        }

        public async Task<ArtistResponse> SearchArtist(string artistName, int pageNumber, int pageSize)
        {
            var artistRequest = new ArtistRequest
            {
                ArtistName = artistName,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            
            using (var operation = _logger.BeginOperation("Fetching artist {ArtistName} paginated information", artistName))
            {
                var artistLists = await _artistRepository.GetAll(artistRequest);
                var artistResponse = new ArtistResponse();
                var artistListDto = new List<ArtistDto>();
                var artistDto = default(ArtistDto);

                foreach (var artist in artistLists)
                {
                    artistDto = new ArtistDto
                    {
                        Name = artist.ArtistName,
                        Country = artist.Country,
                        Alias = artist.Aliases.Split(',').ToList()
                    };
                    artistListDto.Add(artistDto);
                }

                artistResponse.Results.AddRange(artistListDto);
                artistResponse.NumberOfSearchResults = artistLists.Count;
                artistResponse.Page = pageNumber;
                artistResponse.PageSize = pageSize;
                artistResponse.NumberOfPages = (int)Math.Ceiling((double)artistLists.Count / pageSize);
               
                operation.Complete("Artists", artistResponse, true);

                return artistResponse;
            }
        }

        public async Task<AlbumResponse> SearchAlbum(string artistId, int albumCount = 10)
        {
            var httpTransportMessage = new HttpTransportMessage
            {
                Uri = _musicBrainzOptions.Uri,
                RelativePath = $"{_musicBrainzOptions.RelativePath}{artistId}",
                ContentType = "application/json",
            };

            var response = await _httpTransportService.SendAsync(httpTransportMessage);
            var releaseResponse = JsonConvert.DeserializeObject<Root>(response.Content);
            var albumResponse = new AlbumResponse();
            var releases = new List<ReleaseDto>();
            var releaseDto = default(ReleaseDto);
            var otherArtists = new List<FeaturedArtist>();
            var otherArtistDto = default(FeaturedArtist);

            foreach (var release in releaseResponse.releases.Take(albumCount))
            {
                releaseDto = new ReleaseDto
                {
                    ReleaseId = release.id,
                    Title = release.title,
                    Status = release.status,
                    Label = release.LabelInfo?[0].label?.name,
                    NumberOfTracks = release.TrackCount
                };

                if (release.ArtistCredit.Count > 0)
                {
                    foreach (var artist in release.ArtistCredit)
                    {
                        otherArtistDto = new FeaturedArtist
                        {
                            Id = artist.artist.id,
                            Name = artist.artist.name
                        };
                        otherArtists.Add(otherArtistDto);
                    }
                    releaseDto.OtherArtists.AddRange(otherArtists.Except(releaseDto.OtherArtists) ?? new List<FeaturedArtist>());
                    releases.Add(releaseDto);
                    otherArtists = new List<FeaturedArtist>();
                }
                else
                {
                    _logger.Warning("No featured artist found for Release Id {ReleaseId} ", release.id);
                }
            }

            albumResponse.Releases.AddRange(releases);         

            return albumResponse;
        }
    }
}
