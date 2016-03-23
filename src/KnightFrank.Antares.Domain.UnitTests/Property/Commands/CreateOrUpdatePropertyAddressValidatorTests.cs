namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateOrUpdateAddressCommandValidator")]
    [Trait("FeatureTitle", "Address")]
    public class CreateOrUpdatePropertyAddressValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly IFixture fixture;

        public CreateOrUpdatePropertyAddressValidatorTests()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
            this.fixture.Behaviors.Clear();
            this.fixture.RepeatCount = 1;
            this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory]
        [InlineAutoMoqData]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_FieldIsInWrongFormat_Then_ThisFieldShouldBeInValidationResultWithReqExError(
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                CreateOrUpdatePropertyAddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .Without(x => x.AddressFieldDefinitions)
                              .Without(x => x.AddressFormEntityTypes)
                              .Create();

            CreateOrUpdatePropertyAddress query = this.fixture.Build<CreateOrUpdatePropertyAddress>()
                                                      .OmitAutoProperties()
                                                      .With(x => x.City, this.fixture.Create<string>())
                                                      .With(x => x.AddressFormId, addressForm.Id)
                                                      .Create();

            List<PropertyInfo> propertyInfos =
                typeof(CreateOrUpdatePropertyAddress).GetProperties().Where(x => x.PropertyType == typeof(string)).ToList();

            List<AddressFieldDefinition> addressFieldDefinitions = propertyInfos.Select(propertyInfo => this.fixture.BuildAddressFieldDefinition(addressForm, propertyInfo.Name)).ToList();

            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(addressFieldDefinitions);

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(query.City) && e.ErrorCode == "regex_error");
        }

        [Theory]
        [InlineAutoMoqData("")]
        [InlineAutoMoqData((string)null)]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_RequiredFieldIsMissing_Then_ThisFieldShouldBeInValidationError(
                string cityValue,
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                CreateOrUpdatePropertyAddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .Without(x => x.AddressFieldDefinitions)
                              .Without(x => x.AddressFormEntityTypes)
                              .Create();
            CreateOrUpdatePropertyAddress query = this.fixture.Build<CreateOrUpdatePropertyAddress>()
                                                      .OmitAutoProperties()
                                                      .With(x => x.AddressFormId, addressForm.Id)
                                                      .With(x => x.City, cityValue)
                                                      .Create();

            AddressFieldDefinition addressFieldDefinition = this.fixture.BuildAddressFieldDefinition(addressForm, nameof(query.City), ".*", true);

            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(new List<AddressFieldDefinition> { addressFieldDefinition });

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(query.City));
        }

        [Theory]
        [InlineAutoMoqData]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_QueryContainsMoreData_Then_ValidationResultForRedundantFieldShouldBeReturned(
            [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
            CreateOrUpdatePropertyAddressValidator validator)
        {
            // Arrange
            CreateOrUpdatePropertyAddress query = this.fixture.Build<CreateOrUpdatePropertyAddress>()
                .OmitAutoProperties()
                .With(x => x.City, this.fixture.Create<string>())
                .Create();

            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(new List<AddressFieldDefinition>());

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(query.City) && e.ErrorCode == "empty_error");
        }

        [Theory]
        [InlineAutoMoqData]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_InconsistentCountryData_ValidationResultForCountryIdShouldBeSet(
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                [Frozen] Mock<IGenericRepository<AddressForm>> addressFormRepository,
                CreateOrUpdatePropertyAddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .Without(x => x.AddressFieldDefinitions)
                              .Without(x => x.AddressFormEntityTypes)
                              .Create();

            CreateOrUpdatePropertyAddress query = this.fixture.Build<CreateOrUpdatePropertyAddress>()
                                                      .OmitAutoProperties()
                                                      .With(x => x.AddressFormId, addressForm.Id)
                                                      .Create();

            AddressFieldDefinition addressFieldDefinition = this.fixture.BuildAddressFieldDefinition(addressForm, nameof(query.City), ".*", true);

            addressFormRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(addressForm);
            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(new List<AddressFieldDefinition> { addressFieldDefinition });

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(query.CountryId) && e.ErrorMessage == "Inconsistent data");
        }

        [Theory]
        [InlineAutoMoqData]
        public void
            Given_ValidCreateOrUpdateAddressCommand_Then_NoValidationResults(
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                [Frozen] Mock<IGenericRepository<AddressForm>> addressFormRepository,
                CreateOrUpdatePropertyAddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .Without(x => x.AddressFieldDefinitions)
                              .Without(x => x.AddressFormEntityTypes)
                              .Create();

            CreateOrUpdatePropertyAddress query = this.fixture.Build<CreateOrUpdatePropertyAddress>()
                .With(x => x.AddressFormId, addressForm.Id)
                .With(x => x.CountryId, addressForm.CountryId)
                .Create();

            List<PropertyInfo> propertyInfos =
                typeof(CreateOrUpdatePropertyAddress).GetProperties().Where(x => x.PropertyType == typeof(string)).ToList();

            List<AddressFieldDefinition> addressFieldDefinitions = propertyInfos.Select(propertyInfo => this.fixture.BuildAddressFieldDefinition(addressForm, propertyInfo.Name, ".*", true)).ToList();

            addressFormRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(addressForm);
            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(addressFieldDefinitions);

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }


        [Theory]
        [InlineAutoMoqData]
        public void
        Given_InvalidCreateOrUpdateAddressCommand_When_AddressFormIdDoesNotExist_ValidationResultForAddressFormIdShouldBeSet(
            [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
            [Frozen] Mock<IGenericRepository<AddressForm>> addressFormRepository,
            CreateOrUpdatePropertyAddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                  .Without(x => x.AddressFieldDefinitions)
                  .Without(x => x.AddressFormEntityTypes)
                  .Create();

            CreateOrUpdatePropertyAddress query = this.fixture.Build<CreateOrUpdatePropertyAddress>()
                                                      .OmitAutoProperties()
                                                      .Create();

            AddressFieldDefinition addressFieldDefinition = this.fixture.BuildAddressFieldDefinition(addressForm, nameof(query.City), ".*", true);

            addressFormRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns((AddressForm)null);
            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(new List<AddressFieldDefinition> { addressFieldDefinition });

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(query.AddressFormId) && e.ErrorMessage == "Address form does not exist");
        }

    }
}