namespace KnightFrank.Antares.Domain.UnitTests.Viewing.Commands
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Viewing.Commands;

    using Ploeh.AutoFixture;

    using Xunit;

    [Collection("UpdateViewingCommand")]
    [Trait("FeatureTitle", "Viewing ")]
    public class UpdateViewingCommandValidatorTests
    {
        private readonly UpdateViewingCommand cmd;

        public UpdateViewingCommandValidatorTests()
        {
            IFixture fixture = new Fixture();

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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(this.cmd.AttendeesIds));
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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(this.cmd.PostViewingComment));
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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(this.cmd.InvitationText));
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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(this.cmd.StartDate));
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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(this.cmd.EndDate));
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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(this.cmd.EndDate));
        }
    }
}
