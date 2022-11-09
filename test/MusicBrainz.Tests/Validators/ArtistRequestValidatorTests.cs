using AutoFixture;
using FluentValidation.Results;
using WebApi.MusicBrainz.Requests;
using WebApi.MusicBrainz.Validators;

namespace MusicBrainz.Tests.Validators
{
    public class ArtistRequestValidatorTests
    {
        private ArtistRequestValidator _artistRequestValidator;
        private ArtistRequest _artistRequest;
        private ValidationResult _validationResult;
        private Fixture _sut;

        public ArtistRequestValidatorTests()
        {
            _artistRequestValidator = new ArtistRequestValidator();
            _sut = new Fixture();
            InitialiseArtistRequest();
        }

        private void InitialiseArtistRequest()
        {
            _artistRequest = new ArtistRequest
            {
                ArtistName = _sut.Create<string>(),
                PageNumber = _sut.Create<int>(),
                PageSize = _sut.Create<int>(),
            };

        }

        private void AssertValidation(string propertyName)
        {
            var expectedErrorCount = 0;

            _validationResult = _artistRequestValidator.Validate(_artistRequest);

            if (_validationResult.IsValid)
            {
                Assert.Equal(expectedErrorCount, _validationResult.Errors.Count);
                Assert.True(_validationResult.IsValid);
            }
            else
            {
                Assert.False(_validationResult.IsValid);

                var validationErrorsFiltered = _validationResult.Errors.Where(x => x.PropertyName == propertyName);

                foreach (var item in validationErrorsFiltered)
                {
                    Assert.Equal(propertyName, item.PropertyName);
                }
            }
        }

        [Theory]
        [InlineData("")]
        public void GivenEmptyArtistName_ShouldBeInvalid(string artistname)
        {
            _artistRequest.ArtistName = artistname;
            AssertValidation(nameof(_artistRequest.ArtistName));
        }

        public void GivenValidArtistName_ShouldBeValid(string artistname)
        {
            _artistRequest.ArtistName = artistname;
            AssertValidation(nameof(_artistRequest.ArtistName));
        }
    }
}
