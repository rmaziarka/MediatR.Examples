namespace KnightFrank.Antares.Domain.UnitTests.Viewing.Commands
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Viewing.Commands;

    using Ploeh.AutoFixture;

    using Xunit;
    using FixtureExtension;

    using FluentValidation.Resources;

    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    [Collection("UpdateViewingCommand")]
    [Trait("FeatureTitle", "Viewing ")]
    public class UpdateViewingCommandValidatorTests
    {
        private readonly UpdateViewingCommand cmd;

        public UpdateViewingCommandValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();

            this.cmd = fixture.Build<UpdateViewingCommand>()
                              .With(x => x.StartDate, new DateTime(2000, 1, 1, 17, 30, 00))
                              .With(x => x.EndDate, new DateTime(2000, 1, 1, 18, 0, 0))
                              .With(x => x.AttendeesIds, new List<Guid> { Guid.NewGuid() })
                              .Create();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateViewingCommand_When_Validating_Then_IsValid(UpdateViewingCommandValidator validator)
        {
            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyAttendeesIds_When_Validating_Then_IsInvalid(UpdateViewingCommandValidator validator)
        {
            // Arrange
            this.cmd.AttendeesIds.Clear();

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.AttendeesIds), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_TooLongPostViewingComment_When_Validating_Then_IsInvalid(UpdateViewingCommandValidator validator)
        {
            // Arrange
            this.cmd.PostViewingComment = string.Join(string.Empty, new Fixture().CreateMany<char>(4001));

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.PostViewingComment), nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_TooLongInvitationText_When_Validating_Then_IsInvalid(UpdateViewingCommandValidator validator)
        {
            // Arrange
            this.cmd.InvitationText = string.Join(string.Empty, new Fixture().CreateMany<char>(4001));

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.InvitationText), nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyStartDate_When_Validating_Then_IsInvalid(UpdateViewingCommandValidator validator)
        {
            // Arrange
            this.cmd.StartDate = DateTime.MinValue;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.StartDate), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyEndDate_When_Validating_Then_IsInvalid(UpdateViewingCommandValidator validator)
        {
            // Arrange
            this.cmd.EndDate = DateTime.MinValue;

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.EndDate), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EndDateEarlierThanStartDate_When_Validating_Then_IsInvalid(UpdateViewingCommandValidator validator)
        {
            // Arrange
            this.cmd.StartDate = new DateTime(2000, 1, 1, 17, 0, 0);
            this.cmd.EndDate = new DateTime(2000, 1, 1, 16, 0, 0);

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.EndDate), nameof(Messages.greaterthan_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EndDateSameAsStartDate_When_Validating_Then_IsInvalid(UpdateViewingCommandValidator validator)
        {
            // Arrange
            this.cmd.StartDate = new DateTime(2000, 1, 1, 16, 0, 0);
            this.cmd.EndDate = new DateTime(2000, 1, 1, 16, 0, 0);

            // Act
            ValidationResult validationResult = validator.Validate(this.cmd);

            // Assert
            validationResult.IsInvalid(nameof(this.cmd.EndDate), nameof(Messages.greaterthan_error));
        }
    }
}
