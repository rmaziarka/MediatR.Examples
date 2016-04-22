namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using Domain.Property.CommandHandlers;

    using KnightFrank.Antares.Dal.Model.Property;
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
            // Arrange 
            propertyRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((Property)null);

            // Act + Assert
            Assert.Throws<ResourceNotFoundException>(() => handler.Handle(command)).ResourceId.Should().Be(command.Id);
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
            propertyRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>[]>())).Returns(new List<Property> { property });

            // Act
            handler.Handle(command);

            // Assert
            property.Address.ShouldBeEquivalentTo(command.Address, options => options.IncludingProperties().ExcludingMissingMembers());
            propertyRepository.Verify(p => p.Save(), Times.Once);
        }
    }
}