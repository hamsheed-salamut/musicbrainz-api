using FluentValidation;
using WebApi.MusicBrainz.Requests;

namespace WebApi.MusicBrainz.Validators
{
    public class AlbumRequestValidator : AbstractValidator<AlbumRequest>
    {
        public AlbumRequestValidator()
        {
            RuleFor(x => x.ArtistId)
                .NotEmpty()
                .WithMessage("Artist Id cannot be empty")
                .Must(BeValidGuid)
                .WithMessage("Artist Id must be a valid GUID");

            RuleFor(x => x.AlbumCount)
                .NotEmpty()
                .WithMessage("Album count cannot be empty")
                .GreaterThan(0)
                .WithMessage("Album count must be greater than 0");
        }
        private bool BeValidGuid(string content) => Guid.TryParse(content, out Guid result) && result != Guid.Empty;
    }
}
