namespace KnightFrank.Antares.Domain.UnitTests.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using System.Linq.Expressions;
    using System.Reflection;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("AddressValidator")]
    [Trait("FeatureTitle", "Address")]
    public class AddressValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly IFixture fixture;

        public AddressValidatorTests()
        {
            this.fixture = new Fixture().Customize();
        }

        [Theory]
        [InlineAutoMoqData]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_InconsistentCountryData_ShouldThrowBusinessException(
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                [Frozen] Mock<IGenericRepository<AddressForm>> addressFormRepository,
                [Frozen] Mock<IEntityValidator> entityValidator,
                AddressValidator validator)
        {
            // Arrange
            CreateOrUpdateAddress address = this.fixture.Build<CreateOrUpdateAddress>()
                                                      .With(x => x.CountryId, Guid.NewGuid())
                                                      .Create();

            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .With(x => x.CountryId, Guid.NewGuid())
                              .Create();

            addressFormRepository.Setup(x => x.GetById(address.AddressFormId)).Returns(addressForm);

            // Act
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { validator.Validate(address); });

            // Assert
            Assert.Equal(ErrorMessage.Inconsistent_Address_Country_Id, businessValidationException.ErrorCode);
        }

        [Theory]
        [InlineAutoMoqData]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_QueryContainsMoreData_Then_ShouldThrowBusinessException(
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                [Frozen] Mock<IGenericRepository<AddressForm>> addressFormRepository,
                [Frozen] Mock<IEntityValidator> entityValidator,
                AddressValidator validator)
        {
            // Arrange
            CreateOrUpdateAddress address = this.fixture.Build<CreateOrUpdateAddress>()
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
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { validator.Validate(address); });

            // Assert
            Assert.Equal(ErrorMessage.Property_Should_Be_Empty, businessValidationException.ErrorCode);
        }

        [Theory]
        [InlineAutoMoqData]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_FieldIsInWrongFormat_Then_ShouldThrowBusinessException(
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                [Frozen] Mock<IGenericRepository<AddressForm>> addressFormRepository,
                AddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .Without(x => x.AddressFieldDefinitions)
                              .Without(x => x.AddressFormEntityTypes)
                              .Create();

            CreateOrUpdateAddress address = this.fixture.Build<CreateOrUpdateAddress>()
                                                      //.OmitAutoProperties()
                                                      .With(x => x.City, this.fixture.Create<string>())
                                                      .With(x => x.AddressFormId, addressForm.Id)
                                                      .With(x => x.CountryId, addressForm.CountryId)
                                                      .Create();

            addressFormRepository.Setup(x => x.GetById(address.AddressFormId)).Returns(addressForm);

            List<PropertyInfo> propertyInfos =
                typeof(CreateOrUpdateAddress).GetProperties().Where(x => x.PropertyType == typeof(string)).ToList();

            List<AddressFieldDefinition> addressFieldDefinitions = propertyInfos.Select(propertyInfo => this.fixture.BuildAddressFieldDefinition(addressForm, propertyInfo.Name)).ToList();

            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(addressFieldDefinitions);

            // Act
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { validator.Validate(address); });

            // Assert
            Assert.Equal(ErrorMessage.Property_Format_Is_Invalid, businessValidationException.ErrorCode);
        }

        [Theory]
        [InlineAutoMoqData("")]
        [InlineAutoMoqData((string)null)]
        public void
            Given_InvalidCreateOrUpdateAddressCommand_When_RequiredFieldIsMissing_Then_ShouldThrowBusinessException(
                string cityValue,
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                AddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .Without(x => x.AddressFieldDefinitions)
                              .Without(x => x.AddressFormEntityTypes)
                              .Create();
            CreateOrUpdateAddress address = this.fixture.Build<CreateOrUpdateAddress>()
                                                      .OmitAutoProperties()
                                                      .With(x => x.AddressFormId, addressForm.Id)
                                                      .With(x => x.City, cityValue)
                                                      .Create();

            AddressFieldDefinition addressFieldDefinition = this.fixture.BuildAddressFieldDefinition(addressForm, nameof(address.City), ".*", true);

            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(new List<AddressFieldDefinition> { addressFieldDefinition });

            // Act
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { validator.Validate(address); });

            // Assert
            Assert.Equal(ErrorMessage.Property_Should_Not_Be_Empty, businessValidationException.ErrorCode);
        }
        
        [Theory]
        [InlineAutoMoqData]
        public void
            Given_ValidCreateOrUpdateAddressCommand_Then_NoValidationResults(
                [Frozen] Mock<IGenericRepository<AddressFieldDefinition>> addressFieldDefinitionRepository,
                [Frozen] Mock<IGenericRepository<AddressForm>> addressFormRepository,
                [Frozen] Mock<IEntityValidator> entityValidator,
                AddressValidator validator)
        {
            // Arrange
            AddressForm addressForm = this.fixture.Build<AddressForm>()
                              .Without(x => x.AddressFieldDefinitions)
                              .Without(x => x.AddressFormEntityTypes)
                              .Create();

            CreateOrUpdateAddress query = this.fixture.Build<CreateOrUpdateAddress>()
                .With(x => x.AddressFormId, addressForm.Id)
                .With(x => x.CountryId, addressForm.CountryId)
                .Create();

            List<PropertyInfo> propertyInfos =
                typeof(CreateOrUpdateAddress).GetProperties().Where(x => x.PropertyType == typeof(string)).ToList();

            List<AddressFieldDefinition> addressFieldDefinitions = propertyInfos.Select(propertyInfo => this.fixture.BuildAddressFieldDefinition(addressForm, propertyInfo.Name, ".*", true)).ToList();

            addressFormRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(addressForm);
            addressFieldDefinitionRepository
                .Setup(x => x.GetWithInclude(
                    It.IsAny<Expression<Func<AddressFieldDefinition, bool>>>(),
                    It.IsAny<Expression<Func<AddressFieldDefinition, object>>[]>()
                    ))
                .Returns(addressFieldDefinitions);

            // Act
            validator.Validate(query);

            // Assert
            entityValidator.Verify(x => x.EntityExists(It.IsAny<AddressForm>(), query.AddressFormId));
        }
    }
}