namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Activity.Commands;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("UpdateActivityUserCommandValidator")]
    public class UpdateActivityUserCommandValidatorTests
    {
        public static IEnumerable<object[]> FixtureData = new[] { new object[] { DateTime.Now.AddDays(1) }, new object[] { null } };

        [Theory]
        [MemberAutoMoqData("FixtureData", MemberType = typeof(UpdateActivityUserCommandValidatorTests))]
        public void Given_CorrectActivityUserCommand_When_Validating_Then_IsValid(
            DateTime? callDate,
            UpdateActivityUserCommandValidator validator,
            IFixture fixture)
        {
            // Act
            UpdateActivityUserCommand command = fixture.Build<UpdateActivityUserCommand>().With(c => c.CallDate, callDate).Create();

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_UserIdIsEmpty_When_Validating_Then_IsInvalid(
            DateTime? callDate,
            UpdateActivityUserCommandValidator validator,
            IFixture fixture)
        {
            // Arrange
            UpdateActivityUserCommand command =
                fixture.Build<UpdateActivityUserCommand>().With(c => c.Id, Guid.Empty).Create();

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Id));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ActivityIdIsEmpty_When_Validating_Then_IsInvalid(
            DateTime? callDate,
            UpdateActivityUserCommandValidator validator,
            IFixture fixture)
        {
            // Arrange
            UpdateActivityUserCommand command =
                fixture.Build<UpdateActivityUserCommand>()
                       .With(c => c.ActivityId, Guid.Empty)
                       .Create();

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.ActivityId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoData]
        public void Given_CallDateIsPast_When_Validating_Then_IsInvalid(
            UpdateActivityUserCommandValidator validator,
            IFixture fixture)
        {
            // Arrange
            UpdateActivityUserCommand command =
                fixture.Build<UpdateActivityUserCommand>().With(c => c.CallDate, DateTime.Now.AddDays(-1)).Create();

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.CallDate));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == nameof(Messages.greaterthan_error));
        }
    }
}
