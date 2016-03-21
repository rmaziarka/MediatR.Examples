namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{
    using System;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using Domain.Property.CommandHandlers;
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
        public void Given_UpdatePropertyCommand_When_HandleNonExistingProperty_Then_ShouldThrowException(
           UpdatePropertyCommand command,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           UpdatePropertyCommandHandler handler)
        {
            propertyRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((Property)null);

            Assert.Throws<ResourceNotFoundException>(() => handler.Handle(command)).Id.Should().Be(command.Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdatePropertyCommand_When_Handle_Then_ShouldUpdateAddress(
           UpdatePropertyCommand command,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           Property property,
           UpdatePropertyCommandHandler handler)
        {
            // Arrange
            propertyRepository.Setup(p => p.GetById(command.Id)).Returns(property);

            // Act
            handler.Handle(command);

            // Assert
            property.Address.City.Should().Be(command.Address.City);
            property.Address.Line1.Should().Be(command.Address.Line1);
            property.Address.Line2.Should().Be(command.Address.Line2);
            property.Address.Line3.Should().Be(command.Address.Line3);
            property.Address.CountryId.Should().Be(command.Address.CountryId);
            property.Address.County.Should().Be(command.Address.County);
            property.Address.AddressFormId.Should().Be(command.Address.AddressFormId);
            property.Address.Postcode.Should().Be(command.Address.Postcode);
            property.Address.PropertyName.Should().Be(command.Address.PropertyName);
            property.Address.City.Should().Be(command.Address.City);
            property.Address.PropertyNumber.Should().Be(command.Address.PropertyNumber);

            propertyRepository.Verify(p => p.Save(), Times.Once);
        }
    }
}