namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Property.Commands;

    using Xunit;

    public class UpdatePropertyCommandValidatorTests
    {
        [Theory]
        [InlineAutoMoqData]
        public void Given_InvalidCreatePropertyCommand_When_AddressIsNull_Then_TheObjectShouldBeInValidationResultWithReqExError(UpdatePropertyCommandValidator validator)
        {
            // Arrange
            var updatePropertyCommand = new UpdatePropertyCommand();

            // Act
            ValidationResult validationResult = validator.Validate(updatePropertyCommand);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(updatePropertyCommand.Address) && e.ErrorCode == "notnull_error");
        }
    }
}