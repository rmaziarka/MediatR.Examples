namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;
    
    public class CreatePropertyCommandValidatorTest : IClassFixture<BaseTestClassFixture>
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