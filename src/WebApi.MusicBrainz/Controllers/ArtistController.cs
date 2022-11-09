using Core.Common.Models;
using Core.MusicBrainz.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.MusicBrainz.Requests;
using ArtistRequest = WebApi.MusicBrainz.Requests.ArtistRequest;
using ILogger = Serilog.ILogger;

namespace WebApi.MusicBrainz.Controllers
{
    [Route("api/artist")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IArtistService _artistService;
        private readonly IValidator<ArtistRequest> _artistRequestValidator;
        private readonly IValidator<AlbumRequest> _albumRequestValidator;

        public ArtistController(
            ILogger logger,
            IArtistService artistService,
            IValidator<ArtistRequest> artistRequestValidator,
            IValidator<AlbumRequest> albumRequestValidator)
        {
            _logger = logger;
            _artistService = artistService;
            _artistRequestValidator = artistRequestValidator;
            _albumRequestValidator = albumRequestValidator;
        }

        [HttpGet("search/{artistName}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetArtist(string artistName, int pageNumber, int pageSize)
        {
            var artistRequest = new ArtistRequest
            {
                ArtistName = artistName,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var validationResult = await _artistRequestValidator.ValidateAsync(artistRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var artistResponse = new ArtistResponse();

            try
            {
                _logger.Information("Fetching artist name {ArtistName}", artistName);
                artistResponse = await _artistService.SearchArtist(artistName: artistName, pageNumber: pageNumber, pageSize: pageSize);

                return Ok(artistResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while fetching artist name {ArtistName}, error: {ErrorMessage}", artistName, ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("album/{artistId}/{albumCount}")]
        public async Task<IActionResult> GetAlbum(string artistId, int albumCount)
        {
            var albumRequest = new AlbumRequest
            {
                ArtistId = artistId,
                AlbumCount = albumCount
            };

            var validationResult = await _albumRequestValidator.ValidateAsync(albumRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var albumResponse = new AlbumResponse();

            try
            {
                albumResponse = await _artistService.SearchAlbum(artistId: artistId, albumCount);

                return Ok(albumResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while fetching album information for artist Id {ArtistId}, error: {ErrorMessage}", artistId, ex.Message);
                return NotFound(ex.Message);
            }         
        }
    }
}
