namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using ActivityType = KnightFrank.Antares.Dal.Model.Property.Activities.ActivityType;
    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;

    [Trait("FeatureTitle", "Activity")]
    [Collection("UpdateActivityCommandHandler")]
    public class UpdateActivityCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldUpdateActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<ActivityType>> activityTypeRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            [Frozen] Mock<IEntityMapper<Activity>> activityEntityMapper,
            [Frozen] Mock<IActivityTypeDefinitionValidator> activityTypeDefinitionValidator,
            [Frozen] Mock<IAttributeValidator<Tuple<PropertyType, Domain.Common.Enums.ActivityType>>> attributeValidator,
            [Frozen] Mock<IActivityReferenceMapper<Contact>> contactsMapper,
            [Frozen] Mock<IActivityReferenceMapper<ActivityUser>> usersMapper,
            [Frozen] Mock<IActivityReferenceMapper<ActivityDepartment>> departmentsMapper,
            [Frozen] Mock<IActivityReferenceMapper<ActivityAttendee>> attendeesMapper,
            [Frozen] Mock<IActivityReferenceMapper<ChainTransaction>> chainTransactionMapper,
            UpdateActivityCommandHandler handler,
            UpdateActivityCommand command,
            IFixture fixture)
        {
            // Arrange
            var property = fixture.Create<Property>();
            property.Address = fixture.Create<Address>();
            property.PropertyType =
                fixture.Build<Dal.Model.Property.PropertyType>().With(x => x.EnumCode, PropertyType.Flat.ToString()).Create();
            Activity activity = fixture.Build<Activity>()
                                       .With(x => x.Property, property)
                                       .Create();

            activityRepository.Setup(x => x.GetWithInclude(
                It.IsAny<Expression<Func<Activity, bool>>>(),
                It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            ActivityType activityType =
                fixture.Build<ActivityType>()
                       .With(x => x.EnumCode, Domain.Common.Enums.ActivityType.FreeholdSale.ToString())
                       .Create();

            activityTypeRepository.Setup(x => x.GetById(command.ActivityTypeId)).Returns(activityType);

            activityEntityMapper.Setup(x => x.MapAllowedValues(command, activity, PageType.Update)).Returns(activity);

            int callOrder = 0;
            contactsMapper.Setup(x => x.ValidateAndAssign(command, It.IsAny<Activity>())).Callback(() => Assert.Equal(callOrder++, 0)).Verifiable();
            usersMapper.Setup(x => x.ValidateAndAssign(command, It.IsAny<Activity>())).Callback(() => Assert.Equal(callOrder++, 1)).Verifiable();
            departmentsMapper.Setup(x => x.ValidateAndAssign(command, It.IsAny<Activity>())).Callback(() => Assert.Equal(callOrder++, 2)).Verifiable();
            attendeesMapper.Setup(x => x.ValidateAndAssign(command, It.IsAny<Activity>())).Callback(() => Assert.Equal(callOrder++, 3)).Verifiable();
            chainTransactionMapper.Setup(x => x.ValidateAndAssign(command, It.IsAny<Activity>())).Callback(() => Assert.Equal(callOrder++, 4)).Verifiable();

            // Act
            Guid activityId = handler.Handle(command);

            // Assert
            activityId.Should().Be(activity.Id);

            activityEntityMapper.Verify(x => x.MapAllowedValues(command, activity, PageType.Update), Times.Once);

            entityValidator.Verify(x => x.EntityExists(activityType, command.ActivityTypeId), Times.Once);

            attributeValidator.Verify(
                x =>
                    x.Validate(PageType.Update, Tuple.Create(PropertyType.Flat, Domain.Common.Enums.ActivityType.FreeholdSale),
                        command));
            activityTypeDefinitionValidator.Verify(
                x => x.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()),
                Times.Once);

            contactsMapper.Verify();
            usersMapper.Verify();
            departmentsMapper.Verify();
            attendeesMapper.Verify();
            chainTransactionMapper.Verify();
        }
    }
}
