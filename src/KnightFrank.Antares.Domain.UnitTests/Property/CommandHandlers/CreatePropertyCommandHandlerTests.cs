namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{   
    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
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
            propertyToBeSaved.Address.ShouldBeEquivalentTo(command.Address, options => options.IncludingProperties().ExcludingMissingMembers());
            propertyRepository.Verify(x => x.Add(It.IsAny<Property>()), Times.Once);
            propertyRepository.Verify(x => x.Save(), Times.Once);
        }
    }
}