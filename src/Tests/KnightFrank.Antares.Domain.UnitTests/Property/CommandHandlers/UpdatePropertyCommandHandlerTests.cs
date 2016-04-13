namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using Domain.Property.CommandHandlers;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Property.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;
    using Dal.Model.Enum;
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

            Assert.Throws<ResourceNotFoundException>(() => handler.Handle(command)).ResourceId.Should().Be(command.Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdatePropertyCommand_When_Handle_Then_ShouldUpdateAddress(
           UpdatePropertyCommand command,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           Property property,
           UpdatePropertyCommandHandler handler)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(new[] { new EnumTypeItem()});
            // Arrange
            propertyRepository.Setup(p => p.GetById(command.Id)).Returns(property);

            // Act
            handler.Handle(command);

            // Assert
            property.Address.ShouldBeEquivalentTo(command.Address, options => options.IncludingProperties().ExcludingMissingMembers());
            propertyRepository.Verify(p => p.Save(), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_UpdatePropertyCommand_When_HandleInvalidProperty_Then_ShouldThrowException(
           UpdatePropertyCommand command,
           Property property,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           [Frozen] Mock<IGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
           [Frozen] Mock<IDomainValidator<Property>> validator,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           UpdatePropertyCommandHandler handler)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(new[] { new EnumTypeItem() });

            validator.Setup(r => r.Validate(It.IsAny<Property>())).Returns(ValidationResultBuilder.BuildValidationResult());
            
            Assert.Throws<DomainValidationException>(() => handler.Handle(command));
            propertyRepository.Verify(p => p.Save(), Times.Never);
        }
    }
}