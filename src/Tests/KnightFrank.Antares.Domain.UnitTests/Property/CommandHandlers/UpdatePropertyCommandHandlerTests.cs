namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Repository;

    using Domain.Property.CommandHandlers;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Property.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;
    [Collection("UpdatePropertyCommandHandler")]
    [Trait("FeatureTitle", "Property")]
    public class UpdatePropertyCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_UpdatePropertyCommandWithAddress_When_Handle_Then_AddressShouldBeValidated(
           UpdatePropertyCommand command,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           [Frozen] Mock<IAddressValidator> addressValidator,
           UpdatePropertyCommandHandler handler)
        {
            // Arrange
            addressValidator.Setup(x => x.Validate(It.IsAny<CreateOrUpdateAddress>()))
                 .Throws(new BusinessValidationException(It.IsAny<BusinessValidationMessage>()));

            // Act + Assert
            Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });
            addressValidator.Verify(x => x.Validate(It.IsAny<CreateOrUpdateAddress>()), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdatePropertyCommand_When_Handle_Then_ShouldUpdateAddress(
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           [Frozen] Mock<IGenericRepository<PropertyCharacteristic>> propertyCharacteristicRepository,
           [Frozen] Mock<IAddressValidator> addressValidator,
           [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
           [Frozen] Mock<IEntityValidator> entityValidator,
           UpdatePropertyCommandHandler handler,
           UpdatePropertyCommand command,
           Property property)
        {
            property.PropertyCharacteristics = new List<PropertyCharacteristic>();
            // Arrange
            propertyRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>[]>())).Returns(new List<Property> { property });

            // Act
            handler.Handle(command);

            // Assert
            property.Address.ShouldBeEquivalentTo(command.Address, options => options.IncludingProperties().ExcludingMissingMembers());
            entityValidator.Verify(x => x.EntityExists<Dal.Model.Property.PropertyType>(command.PropertyTypeId));
            enumTypeItemValidator.Verify(x => x.ItemExists(EnumType.Division, command.DivisionId));
            entityValidator.Verify(x => x.EntityExists(property, command.Id));
            propertyRepository.Verify(p => p.Save(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdatePropertyCommand_When_DeletingPropertyCharacterictic_Then_ShouldBeMarkedAsDeleted(
           UpdatePropertyCommand command,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           [Frozen] Mock<IGenericRepository<PropertyCharacteristic>> propertyCharacteristicRepository,
           Property property,
           PropertyCharacteristic propertyCharacteristicToDelete,
           UpdatePropertyCommandHandler handler)
        {
            // Arrange
            property.PropertyCharacteristics = new List<PropertyCharacteristic> { propertyCharacteristicToDelete };
            propertyRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>[]>())).Returns(new List<Property> { property });

            // Act
            handler.Handle(command);

            // Assert
            propertyCharacteristicRepository.Verify(r => r.Delete(propertyCharacteristicToDelete));
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdatePropertyCommand_When_AddingPropertyCharacterictic_Then_ShouldBeSaved(
           UpdatePropertyCommand command,
           CreateOrUpdatePropertyCharacteristic propertyCharacteristicToAdd,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           Property property,
           UpdatePropertyCommandHandler handler)
        {
            // Arrange
            property.PropertyCharacteristics = new List<PropertyCharacteristic>();
            command.PropertyCharacteristics.Add(propertyCharacteristicToAdd);
            propertyRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>[]>())).Returns(new List<Property> { property });

            // Act
            handler.Handle(command);

            // Assert
            property.PropertyCharacteristics
                    .SingleOrDefault(pc => pc.CharacteristicId == propertyCharacteristicToAdd.CharacteristicId)
                    .ShouldBeEquivalentTo(propertyCharacteristicToAdd, opt => opt.ExcludingMissingMembers());
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdatePropertyCommand_When_UpdatingPropertyCharacterictic_Then_ShouldBeUpdated(
           UpdatePropertyCommand command,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           Property property,
           Guid characteristicIdToUpdate,
           PropertyCharacteristic propertyCharacteristicToUpdate,
           CreateOrUpdatePropertyCharacteristic newPropertyCharacteristicToUpdate,
           UpdatePropertyCommandHandler handler)
        {
            // Arrange
            property.PropertyCharacteristics = new List<PropertyCharacteristic>();
            propertyCharacteristicToUpdate.CharacteristicId = characteristicIdToUpdate;
            newPropertyCharacteristicToUpdate.CharacteristicId = characteristicIdToUpdate;

            command.PropertyCharacteristics.Add(newPropertyCharacteristicToUpdate);
            property.PropertyCharacteristics.Add(propertyCharacteristicToUpdate);
            propertyRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>[]>())).Returns(new List<Property> { property });

            // Act
            handler.Handle(command);

            // Assert
            property.PropertyCharacteristics
                    .SingleOrDefault(pc => pc.CharacteristicId == characteristicIdToUpdate)
                    .ShouldBeEquivalentTo(newPropertyCharacteristicToUpdate, opt => opt.ExcludingMissingMembers());
        }

    }
}