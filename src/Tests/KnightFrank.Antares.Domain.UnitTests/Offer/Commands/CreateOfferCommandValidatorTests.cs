namespace KnightFrank.Antares.Domain.UnitTests.Offer.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;

    using Xunit;

    [Collection("CreateOfferCommand")]
    [Trait("FeatureTitle", "Offer")]
    public class CreateOfferCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateOfferCommand cmd;

        public CreateOfferCommandValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();   

            this.cmd = fixture.Build<CreateOfferCommand>()
                              .With(x => x.Price, 1)
                              .With(x => x.OfferDate, DateTime.UtcNow.Date)
                              .With(x => x.CompletionDate, DateTime.UtcNow.Date)
                              .With(x => x.ExchangeDate, DateTime.UtcNow.Date)
                              .Create();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateOfferCommand_When_Validating_Then_IsValid(CreateOfferCommandValidator validator)
        {
            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyRequirementId_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.RequirementId = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.RequirementId), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyActivityId_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.ActivityId = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.ActivityId), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyStatusId_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
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
        public void Given_TooLongSpecialConditions_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
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
        public void Given_EmptyOfferDate_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
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
        public void Given_OfferDateInFuture_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
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
        public void Given_EmptyPrice_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
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
        public void Given_PriceLessThanZero_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.Price = -1;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.Price), nameof(Messages.greaterthan_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CompletionDateInPast_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.CompletionDate = DateTime.UtcNow.Date.AddDays(-1);

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.CompletionDate), nameof(Messages.greaterthanorequal_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExchangeDateInPast_When_Validating_Then_IsInvalid(CreateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.ExchangeDate = DateTime.UtcNow.Date.AddDays(-1);

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.ExchangeDate), nameof(Messages.greaterthanorequal_error));
        }
    }
}
