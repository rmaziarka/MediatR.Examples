namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using ActivityType = KnightFrank.Antares.Dal.Model.Property.Activities.ActivityType;
    using PropertyType = KnightFrank.Antares.Dal.Model.Property.PropertyType;

    [Trait("FeatureTitle", "Activity")]
    [Collection("CreateActivityCommandHandler")]
    public class CreateActivityCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldSaveActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<ActivityType>> activityTypeRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IActivityTypeDefinitionValidator> activityTypeDefinitionValidator,
            [Frozen] Mock<IAttributeValidator<Tuple<Domain.Common.Enums.PropertyType, Domain.Common.Enums.ActivityType>>> attributeValidator,
            [Frozen] Mock<IEntityMapper<Activity>> activityEntityMapper,
            CreateActivityCommandHandler handler,
            CreateActivityCommand command,
            Guid expectedActivityId,
            IFixture fixture)
        {
            // Arrange
            User user = this.CreateUser(command.LeadNegotiatorId, fixture);
            Activity activity = null;
            var property = fixture.Create<Property>();
            property.Address = fixture.Create<Address>();
            property.PropertyType =
                fixture.Build<PropertyType>().With(x => x.EnumCode, Domain.Common.Enums.PropertyType.Flat.ToString()).Create();
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

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns(
                                      (Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.CreateEnumTypeItem(ActivityUserType.LeadNegotiator.ToString(), fixture),
                                              this.CreateEnumTypeItem(ActivityDepartmentType.Managing.ToString(), fixture)
                                          }.Where(
                                              expr.Compile()));

            userRepository.Setup(
                p => p.GetWithInclude(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                          .Returns(
                              (Expression<Func<User, bool>> conditionExpression, Expression<Func<User, object>>[] includeExpression) =>
                              new[] { user }.Where(conditionExpression.Compile()));

            activityRepository.Setup(r => r.Add(It.IsAny<Activity>())).Callback((Activity a) => { activity = a; });
            activityRepository.Setup(r => r.Save()).Callback(() => { activity.Id = Guid.NewGuid(); });

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(command.ContactIds.Select(id => new Contact { Id = id }));

            var mappedActivity = new Activity { PropertyId = property.Id, ActivityTypeId = command.ActivityTypeId };
            activityEntityMapper
                .Setup(
                    x =>
                        x.MapAllowedValues(command, It.Is<Activity>(a => a.PropertyId == command.PropertyId && a.ActivityTypeId == command.ActivityTypeId), PageType.Create))
                .Returns(mappedActivity);

            // Act
            Guid activityId = handler.Handle(command);

            // Assert
            activityId.Should().Be(activity.Id);

            activityEntityMapper
                .Verify(
                    x =>
                        x.MapAllowedValues(command, It.Is<Activity>(a => a.PropertyId == command.PropertyId && a.ActivityTypeId == command.ActivityTypeId), PageType.Create), Times.Once);

            activity.ActivityUsers.Should().HaveCount(1);
            ActivityUser leadNegotiator = activity.ActivityUsers.Single();
            leadNegotiator.User.Should().Be(user);
            leadNegotiator.UserType.Code.Should().Be(ActivityUserType.LeadNegotiator.ToString());
            leadNegotiator.CallDate.Should().Be(DateTime.UtcNow.AddDays(14).Date);

            activity.ActivityDepartments.Should().HaveCount(1);
            ActivityDepartment managingDepartment = activity.ActivityDepartments.Single();
            managingDepartment.Department.Should().Be(user.Department);
            managingDepartment.DepartmentType.Code.Should().Be(ActivityDepartmentType.Managing.ToString());

            entityValidator.Verify(x => x.EntityExists(property, command.PropertyId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(user, command.LeadNegotiatorId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(activityType, command.ActivityTypeId), Times.Once);
            attributeValidator.Verify(
                x =>
                    x.Validate(PageType.Create, new Tuple<Domain.Common.Enums.PropertyType, Domain.Common.Enums.ActivityType>(Domain.Common.Enums.PropertyType.Flat,
                        Domain.Common.Enums.ActivityType.FreeholdSale), command));
            activityTypeDefinitionValidator.Verify(
                x => x.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()),
                Times.Once);

            activityRepository.Verify(r => r.Add(It.IsAny<Activity>()), Times.Once());
            activityRepository.Verify(r => r.Save(), Times.Once());
        }

        private EnumTypeItem CreateEnumTypeItem(string code, IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, code).Create();
        }

        private User CreateUser(Guid userId, IFixture fixture)
        {
            var department = fixture.Create<Department>();
            return fixture.Build<User>().With(i => i.Id, userId).With(i => i.Department, department).Create();
        }
    }
}
