namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{
    using System;
    using FluentAssertions;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.CommandHandlers;
    using KnightFrank.Antares.Domain.Property.Commands;

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
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            CreatePropertyCommand command,
            CreatePropertyCommandHandler handler)
        {
            // Arrange
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(new[] { new EnumTypeItem() });
            Property propertyToBeSaved = null;
            propertyRepository.Setup(x => x.Add(It.IsAny<Property>())).Callback<Property>(x => propertyToBeSaved = x);

            // Act
            handler.Handle(command);

            // Assert
            propertyToBeSaved.Should().NotBeNull();
            propertyToBeSaved.Address.ShouldBeEquivalentTo(command.Address, options => options.IncludingProperties().ExcludingMissingMembers());
            propertyRepository.Verify(x => x.Add(It.IsAny<Property>()), Times.Once);
            propertyRepository.Verify(x => x.Save(), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_CreatePropertyCommand_When_HandleInvalidProperty_Then_ShouldThrowException(
           UpdatePropertyCommand command,
           Property property,
           [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
           [Frozen] Mock<IGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           [Frozen] Mock<IDomainValidator<Property>> validator,
           UpdatePropertyCommandHandler handler)
        {
            validator.Setup(r => r.Validate(It.IsAny<Property>())).Returns(ValidationResultBuilder.BuildValidationResult());
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(new[] { new EnumTypeItem() });

            Assert.Throws<DomainValidationException>(() => handler.Handle(command));
            propertyRepository.Verify(p => p.Save(), Times.Never);
            propertyRepository.Verify(p => p.Add(It.IsAny<Property>()), Times.Never);
        }
    }
}