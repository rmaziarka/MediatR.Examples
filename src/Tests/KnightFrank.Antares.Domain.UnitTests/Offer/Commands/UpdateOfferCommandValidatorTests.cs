namespace KnightFrank.Antares.Domain.UnitTests.Offer.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Ploeh.AutoFixture;

    using Xunit;

    [Collection("UpdateOfferCommand")]
    [Trait("FeatureTitle", "Offer")]
    public class UpdateOfferCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly UpdateOfferCommand cmd;

        public UpdateOfferCommandValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();   

            this.cmd = fixture.Build<UpdateOfferCommand>()
                              .With(x => x.Price, 1)
                              .With(x => x.OfferDate, DateTime.UtcNow.Date)
                              .With(x => x.CompletionDate, DateTime.UtcNow.Date)
                              .With(x => x.ExchangeDate, DateTime.UtcNow.Date)
                              .Create();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateOfferCommand_When_Validating_Then_IsValid(UpdateOfferCommandValidator validator)
        {
            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyId_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.Id = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.Id), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyStatusId_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.StatusId = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.StatusId), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_TooLongSpecialConditions_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.SpecialConditions = string.Join(string.Empty, new Fixture().CreateMany<char>(4001));

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.SpecialConditions), nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyOfferDate_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.OfferDate = default(DateTime);

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.OfferDate), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_OfferDateInFuture_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.OfferDate = DateTime.UtcNow.Date.AddDays(1);

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.OfferDate), nameof(Messages.lessthanorequal_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyPrice_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.Price = 0;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.Price), nameof(Messages.greaterthan_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_PriceLessThanZero_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.Price = -1;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.Price), nameof(Messages.greaterthan_error));
        }
    }
}
