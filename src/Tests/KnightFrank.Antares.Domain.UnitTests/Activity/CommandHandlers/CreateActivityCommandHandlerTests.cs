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
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;
    [Trait("FeatureTitle", "Activity")]
    [Collection("CreateActivityCommandHandler")]
    public class CreateActivityCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldSaveActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IActivityTypeDefinitionValidator> activityTypeDefinitionValidator,
            User user,
            CreateActivityCommandHandler handler,
            CreateActivityCommand command,
            Guid expectedActivityId,
            IFixture fixture)
        {
            // Arrange
            Activity activity = null;
            var property = fixture.Create<Property>();
            property.Address = fixture.Create<Address>();
            propertyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(property);

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.CreateEnumTypeItem(EnumTypeItemCode.LeadNegotiator, fixture),
                                              this.CreateEnumTypeItem(EnumTypeItemCode.SecondaryNegotiator, fixture)
                                          }.Where(expr.Compile()));

            userRepository.Setup(p => p.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(new List<User> { user });

            activityRepository.Setup(r => r.Add(It.IsAny<Activity>()))
                              .Returns((Activity a) =>
                              {
                                  activity = a;
                                  return activity;
                              });

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(command.ContactIds.Select(id => new Contact { Id = id }));


            // Act
            handler.Handle(command);

            // Assert
            ActivityUser activityLeadNegotiator = activity.ActivityUsers.SingleOrDefault(u => u.UserType.Code == EnumTypeItemCode.LeadNegotiator);

            activityTypeDefinitionValidator.Verify(x => x.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            activity.Should().NotBeNull();
            activity.ShouldBeEquivalentTo(command, opt => opt.IncludingProperties().ExcludingMissingMembers());

            activityLeadNegotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            activityLeadNegotiator.User.Should().Be(user);
            activityLeadNegotiator.CallDate.Should().HaveValue();

            activityRepository.Verify(r => r.Add(It.IsAny<Activity>()), Times.Once());
            activityRepository.Verify(r => r.Save(), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_Command_When_Handling_Then_EntityExistsValidation_ShouldBeCalledForNegotiator(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<ActivityTypeDefinition>> activityDefinitionRepository,
            [Frozen] Mock<IActivityTypeDefinitionValidator> activityTypeValidator,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            User user,
            CreateActivityCommandHandler handler,
            CreateActivityCommand command,
            IFixture fixture
            )
        {
            // Arrange
            user.Id = command.LeadNegotiatorId;
            var property = fixture.Create<Property>();
            property.Address = fixture.Create<Address>();
            propertyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(property);
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.CreateEnumTypeItem(EnumTypeItemCode.LeadNegotiator, fixture),
                                              this.CreateEnumTypeItem(EnumTypeItemCode.SecondaryNegotiator, fixture)
                                          }.Where(expr.Compile()));

            activityDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>()))
                                        .Returns(true);
            userRepository.Setup(p => p.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(new List<User> { user });

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                 .Returns(command.ContactIds.Select(id => new Contact { Id = id }));
            entityValidator.Setup(x => x.EntityExists(user, user.Id));
            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists(user, user.Id), Times.Once);
        }

        private EnumTypeItem CreateEnumTypeItem(string code, IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, code).Create();
        }
    }
}