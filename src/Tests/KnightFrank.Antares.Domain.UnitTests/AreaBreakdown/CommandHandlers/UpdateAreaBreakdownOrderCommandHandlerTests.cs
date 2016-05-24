namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("UpdateAreaBreakdownOrderCommandHandler")]
    public class UpdateAreaBreakdownOrderCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]

        [InlineAutoMoqData(0, 1, new[] { 1, 0, 2 })]
        [InlineAutoMoqData(0, 2, new[] { 2, 0, 1 })]
        [InlineAutoMoqData(2, 0, new[] { 1, 2, 0 })]
        [InlineAutoMoqData(2, 1, new[] { 0, 2, 1 })]
        [InlineAutoMoqData(1, 0, new[] { 1, 0, 2 })]
        [InlineAutoMoqData(1, 2, new[] { 0, 2, 1 })]
        public void Given_CorrectCommand_When_Handle_Then_ShouldChangeOrder(
            int order,
            int destinationOrder,
            int[] expectedOrder,
            Property property,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            UpdateAreaBreakdownOrderCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            PropertyAreaBreakdown areaA = this.CreatePropertyAreaBreakdown(0, property.Id, fixture);
            PropertyAreaBreakdown areaB = this.CreatePropertyAreaBreakdown(1, property.Id, fixture);
            PropertyAreaBreakdown areaC = this.CreatePropertyAreaBreakdown(2, property.Id, fixture);
            PropertyAreaBreakdown areaD = this.CreatePropertyAreaBreakdown(0, Guid.NewGuid(), fixture);

            PropertyAreaBreakdown updatedArea = new[] { areaA, areaB, areaC }[order];
            UpdateAreaBreakdownOrderCommand command = this.CreateUpdateAreaBreakdownOrderCommand(
                updatedArea,
                destinationOrder,
                property.Id,
                fixture);

            propertyRepository.Setup(r => r.GetById(command.PropertyId)).Returns(property);
            areaBreakdownRepository.Setup(r => r.GetById(command.AreaId)).Returns(updatedArea);

            areaBreakdownRepository.Setup(r => r.FindBy(It.IsAny<Expression<Func<PropertyAreaBreakdown, bool>>>()))
                                   .Returns(
                                       (Expression<Func<PropertyAreaBreakdown, bool>> exp) =>
                                       new[] { areaC, areaB, areaA, areaD }.Where(exp.Compile()));

            PropertyAreaBreakdown[] savedAreas = null;
            areaBreakdownRepository.Setup(r => r.Save()).Callback(() => { savedAreas = new[] { areaA, areaB, areaC }; });

            // Act
            Guid guid = handler.Handle(command);

            // Assert
            guid.Should().NotBeEmpty();
            guid.Should().Be(updatedArea.Id);
            propertyRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            areaBreakdownRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            areaBreakdownRepository.Verify(x => x.FindBy(It.IsAny<Expression<Func<PropertyAreaBreakdown, bool>>>()), Times.Once);
            entityValidator.Verify(x => x.EntityExists(property, command.PropertyId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(It.IsAny<Property>(), It.IsAny<Guid>()), Times.Once);
            entityValidator.Verify(x => x.EntityExists(updatedArea, command.AreaId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(It.IsAny<PropertyAreaBreakdown>(), It.IsAny<Guid>()), Times.Once);

            areaBreakdownRepository.Verify(x => x.Save(), Times.Once);
            savedAreas.Should().NotBeNull();
            savedAreas.Single(x => x.Id == areaA.Id).Order.Should().Be(expectedOrder[0]);
            savedAreas.Single(x => x.Id == areaB.Id).Order.Should().Be(expectedOrder[1]);
            savedAreas.Single(x => x.Id == areaC.Id).Order.Should().Be(expectedOrder[2]);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommandWithIncorrectOrder_When_Handle_Then_ShouldThrowException(
            IList<PropertyAreaBreakdown> propertyAreaBreakdowns,
            UpdateAreaBreakdownOrderCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            UpdateAreaBreakdownOrderCommand command = this.CreateUpdateAreaBreakdownOrderCommand(
                propertyAreaBreakdowns[0],
                propertyAreaBreakdowns.Count + 1,
                Guid.NewGuid(),
                fixture);

            // Act
            Action act = () => handler.Handle(command);

            // Asset
            act.ShouldThrow<BusinessValidationException>()
               .Which.ErrorCode.ShouldBeEquivalentTo(ErrorMessage.PropertyAreaBreakdown_OrderOutOfRange);
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectCommandWithDifferentPropertyId_When_Handle_Then_ShouldBeBusinessValidationExceptionThrown(
            [Frozen] Mock<IGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            Property property,
            PropertyAreaBreakdown propertyAreaBreakdown,
            UpdateAreaBreakdownOrderCommand command,
            UpdateAreaBreakdownOrderCommandHandler handler)
        {
            // Arrange
            propertyRepository.Setup(r => r.GetById(command.PropertyId)).Returns(property);
            areaBreakdownRepository.Setup(r => r.GetById(command.AreaId)).Returns(propertyAreaBreakdown);

            // Act
            Action act = () => handler.Handle(command);

            // Assert
            act.ShouldThrow<BusinessValidationException>()
               .Which.ErrorCode.ShouldBeEquivalentTo(ErrorMessage.PropertyAreaBreakdown_Is_Assigned_To_Other_Property);
        }

        private UpdateAreaBreakdownOrderCommand CreateUpdateAreaBreakdownOrderCommand(
            PropertyAreaBreakdown area,
            int order,
            Guid propertyId,
            IFixture fixture)
        {
            return
                fixture.Build<UpdateAreaBreakdownOrderCommand>()
                       .With(x => x.AreaId, area.Id)
                       .With(x => x.PropertyId, propertyId)
                       .With(x => x.Order, order)
                       .Create();
        }

        private PropertyAreaBreakdown CreatePropertyAreaBreakdown(int order, Guid propertyId, IFixture fixture)
        {
            return fixture.Build<PropertyAreaBreakdown>().With(x => x.Order, order).With(x => x.PropertyId, propertyId).Create();
        }
    }
}
