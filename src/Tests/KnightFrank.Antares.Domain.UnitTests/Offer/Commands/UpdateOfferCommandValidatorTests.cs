namespace KnightFrank.Antares.Domain.UnitTests.Offer.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

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
                              .With(x => x.MortgageLoanToValue, 1)
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

        [Theory]
        [InlineAutoData(-200, nameof(Messages.greaterthanorequal_error))]
        [InlineAutoData(-1, nameof(Messages.greaterthanorequal_error))]
        [InlineAutoData(201, nameof(Messages.lessthanorequal_error))]
        [InlineAutoData(501, nameof(Messages.lessthanorequal_error))]
        public void Given_InvalidMortgageLoanToValueICommand_When_Validating_Then_CorrectErrorCodeIsReturned(int mortgageLoanToValue, string errorCode, UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.MortgageLoanToValue = mortgageLoanToValue;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.MortgageLoanToValue), errorCode);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NullMortgageLoanToValueInCommand_When_Validating_Then_IsValid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.MortgageLoanToValue = null;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineAutoData(0)]
        [InlineAutoData(55)]
        [InlineAutoData(200)]
        public void Given_ValidMortgageLoanToValueInCommand_When_Validating_Then_IsValid(int mortgageLoanToValue, UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.MortgageLoanToValue = mortgageLoanToValue;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData("example comment")]
        public void Given_EmptyProgressCommentInCommand_When_Validating_Then_IsValid(string progressComment, UpdateOfferCommandValidator validator)
        {
            // Arrange
            this.cmd.ProgressComment = progressComment;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_MaxAllowedCharInProgressCommentInCommand_When_Validating_Then_IsValid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            int maxAllowed = 4000;
            this.cmd.ProgressComment = string.Join(string.Empty, new Fixture().CreateMany<char>(maxAllowed));

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_TooLongProgressCommentInCommand_When_Validating_Then_IsInvalid(UpdateOfferCommandValidator validator)
        {
            // Arrange
            int maxAllowed = 4000;
            this.cmd.ProgressComment = string.Join(string.Empty, new Fixture().CreateMany<char>(maxAllowed + 1));

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.ProgressComment), nameof(Messages.length_error));
        }
    }
}