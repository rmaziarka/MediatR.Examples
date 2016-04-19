namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Collection("CreatePropertyCommandValidator")]
    [Trait("FeatureTitle", "Property")]
    public class CreatePropertyCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository;        
        private readonly Mock<IGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository;
        private readonly Mock<IGenericRepository<AddressForm>> addressFormRepository;
        private readonly CreatePropertyCommand command;
        private readonly CreatePropertyCommandValidator validator;

        public CreatePropertyCommandValidatorTests()
        {
            IFixture fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.command = fixture.Build<CreatePropertyCommand>()
                                  .With(p => p.Address, new CreateOrUpdatePropertyAddress())
                                  .With(p => p.PropertyTypeId, Guid.NewGuid())
                                  .With(p => p.DivisionId, Guid.NewGuid())
                                  .Create();

            this.enumTypeItemRepository = fixture.Freeze<Mock<IGenericRepository<EnumTypeItem>>>();
            this.propertyTypeDefinitionRepository = fixture.Freeze<Mock<IGenericRepository<PropertyTypeDefinition>>>();
            this.addressFormRepository = fixture.Freeze<Mock<IGenericRepository<AddressForm>>>();

            this.validator = fixture.Create<CreatePropertyCommandValidator>();
        }

        [Fact]
        public void Given_InvalidCreatePropertyCommand_When_AddressIsNull_Then_TheObjectShouldBeInValidationResultWithReqExError()
        {
            // Arrange
            var createPropertyCommand = new CreatePropertyCommand();

            // Act
            ValidationResult validationResult = this.validator.Validate(createPropertyCommand);

            // Assert
            validationResult.Errors.Should()
                            .Contain(e => e.PropertyName == nameof(createPropertyCommand.Address) && e.ErrorCode == "notnull_error");
        }

        [Fact]
        public void Given_ValidCreatePropertyCommand_When_DivisionExists_Then_NoError()
        {
            // Arrange
            this.enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            this.propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                .Returns(true);

            // Act
            ValidationResult validationResult = this.validator.Validate(this.command);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_ValidCreatePropertyCommand_When_DivisionNotExists_Then_Error()
        {
            // Arrange
            this.enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(false);
            this.propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                .Returns(true);

            // Act
            ValidationResult validationResult = this.validator.Validate(this.command);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void Given_PropertyTypeIsCorrect_When_Validating_Then_NoValidationErrors()
        {
            // Arrange
            Guid countryId = Guid.NewGuid();
            this.command.Address.CountryId = countryId;

            var propertyTypeDefinitions = new List<PropertyTypeDefinition>
            {
                new PropertyTypeDefinition
                {
                    CountryId = this.command.Address.CountryId,
                    DivisionId = this.command.DivisionId,
                    PropertyTypeId = this.command.PropertyTypeId
                }
            };
            
            this.addressFormRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new AddressForm { CountryId = countryId });
            this.enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            this.propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                .Returns(
                    new Func<Expression<Func<PropertyTypeDefinition, bool>>, bool>(
                        expr => propertyTypeDefinitions.Any(expr.Compile())));

            // Act
            ValidationResult validationResult = this.validator.Validate(this.command);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_PropertyTypeIsNotValidForGivenCountry_When_Validating_Then_ValidationErrors()
        {
            // Arrange
            Guid countryId = Guid.NewGuid();
            this.command.Address.CountryId = countryId;

            var propertyTypeDefinitions = new List<PropertyTypeDefinition>
            {
                new PropertyTypeDefinition
                {
                    CountryId = Guid.Empty,
                    DivisionId = this.command.DivisionId,
                    PropertyTypeId = this.command.PropertyTypeId
                },
                new PropertyTypeDefinition
                {
                    CountryId = this.command.Address.CountryId,
                    DivisionId = Guid.Empty,
                    PropertyTypeId = this.command.PropertyTypeId
                },
                new PropertyTypeDefinition
                {
                    CountryId = this.command.Address.CountryId,
                    DivisionId = this.command.DivisionId,
                    PropertyTypeId = Guid.Empty
                }
            };
            
            this.addressFormRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new AddressForm { CountryId = countryId });
            this.enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            this.propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                .Returns(
                    new Func<Expression<Func<PropertyTypeDefinition, bool>>, bool>(
                        expr => propertyTypeDefinitions.Any(expr.Compile())));

            // Act 
            ValidationResult validationResult = this.validator.Validate(this.command);

            // Assert
            Assert.False(validationResult.IsValid);
        }
    }
}
