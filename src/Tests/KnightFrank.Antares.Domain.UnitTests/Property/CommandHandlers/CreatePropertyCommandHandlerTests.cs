namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{
    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Property.CommandHandlers;
    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreatePropertyCommandHandler")]
    [Trait("FeatureTitle", "Property")]
    public class CreatePropertyCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CreatePropertyCommand_When_Handle_Then_ShouldCreateProperty(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IAddressValidator> addressValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IEntityValidator> entityValidator,
            CreatePropertyCommand command,
            CreatePropertyCommandHandler handler)
        {
            // Arrange
            Property propertyToBeSaved = null;
            propertyRepository.Setup(x => x.Add(It.IsAny<Property>())).Callback<Property>(x => propertyToBeSaved = x);

            // Act
            handler.Handle(command);

            // Assert
            propertyToBeSaved.Should().NotBeNull();
            entityValidator.Verify(x=>x.EntityExists<Dal.Model.Property.PropertyType>(command.PropertyTypeId));
            enumTypeItemValidator.Verify(x => x.ItemExists(EnumType.Division, command.DivisionId));
            propertyToBeSaved.Address.ShouldBeEquivalentTo(command.Address, options => options.IncludingProperties().ExcludingMissingMembers());
            propertyRepository.Verify(x => x.Add(It.IsAny<Property>()), Times.Once);
            propertyRepository.Verify(x => x.Save(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreatePropertyCommandWithAddress_When_Handle_Then_AddressShouldBeValidated(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IAddressValidator> addressValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IEntityValidator> entityValidator,
            CreatePropertyCommand command,
            CreatePropertyCommandHandler handler)
        {
            // Arrange
            addressValidator.Setup(x => x.Validate(It.IsAny<CreateOrUpdateAddress>()))
                            .Throws(new BusinessValidationException(It.IsAny<BusinessValidationMessage>()));

            // Act + Assert
            Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });
            addressValidator.Verify(x => x.Validate(It.IsAny<CreateOrUpdateAddress>()), Times.Once());
        }
    }
}