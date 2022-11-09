using FluentValidation;
using WebApi.MusicBrainz.Requests;

namespace WebApi.MusicBrainz.Validators
{
    public class ArtistRequestValidator : AbstractValidator<ArtistRequest>
    {
        public ArtistRequestValidator()
        {
            RuleFor(x => x.ArtistName)
                .NotEmpty()
                .WithMessage("Artist Name cannot be empty");

            RuleFor(x => x.PageNumber)
                .NotEmpty()
                .WithMessage("Page Number cannot be empty")
                .GreaterThan(0)
                .WithMessage("Page Number must be greater than 0");

            RuleFor(x => x.PageSize)
                .NotEmpty()
                .WithMessage("Page Size cannot be empty")
                .GreaterThan(0)
                .WithMessage("Page Size must be greater than 0");
        }
    }
}
