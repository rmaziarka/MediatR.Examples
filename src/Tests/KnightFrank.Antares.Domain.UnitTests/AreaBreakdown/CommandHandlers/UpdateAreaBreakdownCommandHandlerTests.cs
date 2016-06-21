namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.CommandHandlers
{
    using System;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("UpdateAreaBreakdownCommandHandler")]
    public class UpdateAreaBreakdownCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldUpdateNameAndSizeAndReturnValidId(
            Property property,
            PropertyAreaBreakdown propertyAreaBreakdown,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandHandler handler)
        {
            // Arrange
            propertyAreaBreakdown.PropertyId = property.Id;
            PropertyAreaBreakdown savedPropertyAreaBreakdown = null;
            propertyRepository.Setup(r => r.GetById(command.PropertyId)).Returns(property);
            areaBreakdownRepository.Setup(r => r.GetById(command.Id)).Returns(propertyAreaBreakdown);
            areaBreakdownRepository.Setup(r => r.Save()).Callback(() => { savedPropertyAreaBreakdown = propertyAreaBreakdown; });

            // Act
            Guid guid = handler.Handle(command);

            // Assert
            guid.Should().NotBeEmpty();
            guid.Should().Be(propertyAreaBreakdown.Id);
            propertyRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            areaBreakdownRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            entityValidator.Verify(x => x.EntityExists(property, command.PropertyId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(It.IsAny<Property>(), It.IsAny<Guid>()), Times.Once);
            entityValidator.Verify(x => x.EntityExists(propertyAreaBreakdown, command.Id), Times.Once);
            entityValidator.Verify(x => x.EntityExists(It.IsAny<PropertyAreaBreakdown>(), It.IsAny<Guid>()), Times.Once);
            areaBreakdownRepository.Verify(x => x.Save(), Times.Once);
            savedPropertyAreaBreakdown.Should().NotBeNull();
            savedPropertyAreaBreakdown.Name.Should().Be(command.Name);
            savedPropertyAreaBreakdown.Size.Should().Be(command.Size);
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectCommandWithDifferentPropertyId_When_Handle_Then_ShouldBeBusinessValidationExceptionThrown(
            [Frozen] Mock<IGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            Property property,
            PropertyAreaBreakdown propertyAreaBreakdown,
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandHandler handler)
        {
            // Arrange
            propertyRepository.Setup(r => r.GetById(command.PropertyId)).Returns(property);
            areaBreakdownRepository.Setup(r => r.GetById(command.Id)).Returns(propertyAreaBreakdown);

            // Act
            Action act = () => handler.Handle(command);

            // Assert
            act.ShouldThrow<BusinessValidationException>().Which.ErrorCode.ShouldBeEquivalentTo(ErrorMessage.PropertyAreaBreakdown_Is_Assigned_To_Other_Property);
        }
    }
}
