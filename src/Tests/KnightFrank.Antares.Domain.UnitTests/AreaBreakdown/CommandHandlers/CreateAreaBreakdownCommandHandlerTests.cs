namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("CreateAreaBreakdownCommandHandler")]
    public class CreateAreaBreakdownCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Fixture fixture;

        private readonly Property property;

        private readonly EnumTypeItem commercialEnumTypeItem;

        public CreateAreaBreakdownCommandHandlerTests()
        {
            this.fixture = new Fixture().Customize();

            this.property = this.fixture.Build<Property>()
                           .With(x => x.PropertyAreaBreakdowns, this.fixture.CreateMany<PropertyAreaBreakdown>(5).ToList())
                           .Create();

            this.commercialEnumTypeItem = this.fixture.Build<EnumTypeItem>().With(x => x.Code, DivisionEnum.Commercial.ToString()).Create();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateAreaBreakdownCommand_When_Handling_Then_ValidatorsAreInvoked(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            CreateAreaBreakdownCommand command,
            CreateAreaBreakdownCommandHandler handler)
        {
            this.property.Id = command.PropertyId;

            enumTypeItemRepository.Setup(x => x.GetById(this.property.DivisionId)).Returns(this.commercialEnumTypeItem);
            propertyRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>>()))
                              .Returns(new[] { this.property });

            handler.Handle(command);

            enumTypeItemRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            propertyRepository.Verify(x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>>()), Times.Once());
            entityValidator.Verify(x => x.EntityExists(this.property, command.PropertyId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(It.IsAny<Property>(), It.IsAny<Guid>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateAreaBreakdownCommand_WithPropertyTypeDifferentThanCommercial_When_Handling_Then_BusinessValidationExceptionShouldBeThrown(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            EnumTypeItem enumTypeItem,
            CreateAreaBreakdownCommand command,
            CreateAreaBreakdownCommandHandler handler)
        {
            this.property.Id = command.PropertyId;
            
            enumTypeItemRepository.Setup(x => x.GetById(this.property.DivisionId)).Returns(enumTypeItem);
            propertyRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>>()))
                              .Returns(new[] { this.property });

            Action act = () => handler.Handle(command);

            act.ShouldThrow<BusinessValidationException>().And.ErrorCode.ShouldBeEquivalentTo(ErrorMessage.Only_Commercial_Property_Should_Have_AreaBreakdown);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateAreaBreakdownCommand_When_Handling_Then_AreasShouldHaveRightOrder(
            [Frozen] Mock<IGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            CreateAreaBreakdownCommand command,
            CreateAreaBreakdownCommandHandler handler)
        {
            this.property.Id = command.PropertyId;
            var expectedOrder = new List<int> { 5, 6, 7 };
            var newlyCreatedAreasOrder = new List<int>();

            enumTypeItemRepository.Setup(x => x.GetById(this.property.DivisionId)).Returns(this.commercialEnumTypeItem);
            propertyRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>>()))
                              .Returns(new[] { this.property });

            areaBreakdownRepository.Setup(x => x.Add(It.IsAny<PropertyAreaBreakdown>()))
                                   .Callback<PropertyAreaBreakdown>(x => newlyCreatedAreasOrder.Add(x.Order));

            handler.Handle(command);

            newlyCreatedAreasOrder.ShouldAllBeEquivalentTo(expectedOrder);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateAreaBreakdownCommand_When_Handling_Then_PropertyShouldHaveUpdatedTotalAreaBreakdown(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            CreateAreaBreakdownCommand command,
            CreateAreaBreakdownCommandHandler handler)
        {
            this.property.Id = command.PropertyId;
            double? expectedTotalAreaBreakdown = this.property.TotalAreaBreakdown + command.Areas.Sum(x => x.Size);

            enumTypeItemRepository.Setup(x => x.GetById(this.property.DivisionId)).Returns(this.commercialEnumTypeItem);
            propertyRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>>()))
                              .Returns(new[] { this.property });

            handler.Handle(command);

            this.property.TotalAreaBreakdown.ShouldBeEquivalentTo(expectedTotalAreaBreakdown);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateAreaBreakdownCommand_When_Handling_Then_PropertyShouldHaveTotalAreaBreakdownSetToZeroWhenIsNull(
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            CreateAreaBreakdownCommand command,
            CreateAreaBreakdownCommandHandler handler)
        {
            this.property.Id = command.PropertyId;
            this.property.TotalAreaBreakdown = null;
            double expectedTotalAreaBreakdown = command.Areas.Sum(x => x.Size);

            enumTypeItemRepository.Setup(x => x.GetById(this.property.DivisionId)).Returns(this.commercialEnumTypeItem);
            propertyRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>>()))
                              .Returns(new[] { this.property });

            handler.Handle(command);

            this.property.TotalAreaBreakdown.ShouldBeEquivalentTo(expectedTotalAreaBreakdown);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateAreaBreakdownCommand_When_Handling_Then_AreaBreakdownAndPropertyShouldBeSavedAndListOfIdsShouldBeReturned(
            [Frozen] Mock<IGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            CreateAreaBreakdownCommand command,
            CreateAreaBreakdownCommandHandler handler)
        {
            this.property.Id = command.PropertyId;
            var newlyCreatedIds = new List<Guid>();

            enumTypeItemRepository.Setup(x => x.GetById(this.property.DivisionId)).Returns(this.commercialEnumTypeItem);
            propertyRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, object>>>()))
                              .Returns(new[] { this.property });

            areaBreakdownRepository.Setup(x => x.Add(It.IsAny<PropertyAreaBreakdown>()))
                                   .Callback<PropertyAreaBreakdown>(
                                       x =>
                                           {
                                               x.Id = Guid.NewGuid();
                                               newlyCreatedIds.Add(x.Id);
                                           });

            IList<Guid> result = handler.Handle(command);

            result.ShouldAllBeEquivalentTo(newlyCreatedIds);
            areaBreakdownRepository.Verify(x => x.Add(It.IsAny<PropertyAreaBreakdown>()), Times.Exactly(command.Areas.Count));
            areaBreakdownRepository.Verify(x => x.Save(), Times.Once);
        }
    }
}