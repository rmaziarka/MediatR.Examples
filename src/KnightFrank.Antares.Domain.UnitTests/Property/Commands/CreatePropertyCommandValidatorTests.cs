namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Property.Commands;

    using Xunit;
    
    public class CreatePropertyCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {   
        [Theory]
        [InlineAutoMoqData]
        public void Given_InvalidCreatePropertyCommand_When_AddressIsNull_Then_TheObjectShouldBeInValidationResultWithReqExError(CreatePropertyCommandValidator validator)
        {
            // Arrange
            var createPropertyCommand = new CreatePropertyCommand();
            
            // Act
            ValidationResult validationResult = validator.Validate(createPropertyCommand);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(createPropertyCommand.Address) && e.ErrorCode == "notnull_error");
        }
    }
}