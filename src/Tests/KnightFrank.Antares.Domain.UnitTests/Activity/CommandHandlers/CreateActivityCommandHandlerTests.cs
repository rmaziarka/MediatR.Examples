namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;

    [Trait("FeatureTitle", "Activity")]
    [Collection("CreateActivityCommandHandler")]
    public class CreateActivityCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldSaveActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<ActivityType>> activityTypeRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IActivityTypeDefinitionValidator> activityTypeDefinitionValidator,
            [Frozen] Mock<IAttributeValidator<Tuple<PropertyType, Domain.Common.Enums.ActivityType>>> attributeValidator,
            [Frozen] Mock<IEntityMapper<Activity>> activityEntityMapper,
            CreateActivityCommandHandler handler,
            CreateActivityCommand command,
            Guid expectedActivityId,
            IFixture fixture)
        {
            // Arrange
            Activity activity = null;
            var property = fixture.Create<Property>();
            property.Address = fixture.Create<Address>();
            property.PropertyType =
                fixture.Build<Dal.Model.Property.PropertyType>().With(x => x.EnumCode, PropertyType.Flat.ToString()).Create();
            propertyRepository.Setup(
                x => x.GetWithInclude(
                    It.IsAny<Expression<Func<Property, bool>>>(),
                    It.IsAny<Expression<Func<Property, object>>>(),
                    It.IsAny<Expression<Func<Property, object>>>())).Returns(new[] { property });
            ActivityType activityType =
                fixture.Build<ActivityType>()
                       .With(x => x.EnumCode, Domain.Common.Enums.ActivityType.FreeholdSale.ToString())
                       .Create();
            activityTypeRepository.Setup(x => x.GetById(command.ActivityTypeId)).Returns(activityType);
            activityRepository.Setup(r => r.Add(It.IsAny<Activity>())).Callback((Activity a) => { activity = a; });
            activityRepository.Setup(r => r.Save()).Callback(() => { activity.Id = Guid.NewGuid(); });

            var mappedActivity = new Activity { PropertyId = property.Id, ActivityTypeId = command.ActivityTypeId };
            activityEntityMapper
                .Setup(
                    x =>
                        x.MapAllowedValues(command,
                            It.Is<Activity>(a => a.PropertyId == command.PropertyId && a.ActivityTypeId == command.ActivityTypeId),
                            PageType.Create))
                .Returns(mappedActivity);

            // Act
            Guid activityId = handler.Handle(command);

            // Assert
            activityId.Should().Be(activity.Id);

            activityEntityMapper
                .Verify(
                    x =>
                        x.MapAllowedValues(command,
                            It.Is<Activity>(a => a.PropertyId == command.PropertyId && a.ActivityTypeId == command.ActivityTypeId),
                            PageType.Create), Times.Once);

            entityValidator.Verify(x => x.EntityExists(property, command.PropertyId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(activityType, command.ActivityTypeId), Times.Once);
            attributeValidator.Verify(
                x =>
                    x.Validate(PageType.Create, new Tuple<PropertyType, Domain.Common.Enums.ActivityType>(PropertyType.Flat,
                        Domain.Common.Enums.ActivityType.FreeholdSale), command));
            activityTypeDefinitionValidator.Verify(
                x => x.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()),
                Times.Once);

            activityRepository.Verify(r => r.Add(It.IsAny<Activity>()), Times.Once());
            activityRepository.Verify(r => r.Save(), Times.Once());
        }
    }
}
