namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
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
    [Collection("UpdateActivityCommandHandler")]
    public class UpdateActivityCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldUpdateActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<ActivityTypeDefinition>> activityTypeDefinitionRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser>();
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            activityTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>())).Returns(true);
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                          }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            activity.ShouldBeEquivalentTo(
                command,
                options =>
                options.Including(x => x.ActivityStatusId)
                       .Including(x => x.MarketAppraisalPrice)
                       .Including(x => x.RecommendedPrice)
                       .Including(x => x.VendorEstimatedPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_EntityExistsValidation_ShouldBeCalledForActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommandHandler handler,
            UpdateActivityCommand command,
            IFixture fixture
            )
        {
            // Arrange
            Activity activity = this.GetActivity(fixture);
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                          }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists(activity, activity.Id), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_EntityExistsValidation_ShouldBeCalledForLeadNegotiator(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            UpdateActivityCommandHandler handler,
            UpdateActivityCommand command,
            IFixture fixture
            )
        {
            // Arrange
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser>();
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                          }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists<User>(command.LeadNegotiator.UserId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_EntityExistsValidation_ShouldBeCalledForSecondaryNegotiators(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            UpdateActivityCommandHandler handler,
            UpdateActivityCommand command,
            IFixture fixture
            )
        {
            // Arrange
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser>();
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                          }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntitiesExist<User>(It.Is<List<Guid>>( list => list.SequenceEqual(command.SecondaryNegotiators.Select(n => n.UserId).ToList()))), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_CollectionIsUniqueValidation_ShouldBeCalledForAllNegotiators(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            UpdateActivityCommandHandler handler,
            UpdateActivityCommand command,
            IFixture fixture
            )
        {
            // Arrange
            var calledNegotiators = new List<Guid>();
            Activity activity = this.GetActivity(fixture);
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            collectionValidator.Setup(x => x.CollectionIsUnique(It.IsAny<ICollection<Guid>>(), It.IsAny<ErrorMessage>()))
                               .Callback((ICollection<Guid> list, ErrorMessage error) => { calledNegotiators.AddRange(list); });
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                          {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                          }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            IList<Guid> expectedNegotiators = command.SecondaryNegotiators.Select(n => n.UserId).ToList();
            expectedNegotiators.Add(command.LeadNegotiator.UserId);

            calledNegotiators.ShouldAllBeEquivalentTo(expectedNegotiators);
            collectionValidator.Verify(x => x.CollectionIsUnique(It.IsAny<ICollection<Guid>>(), ErrorMessage.Activity_Negotiators_Not_Unique), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_DeletingSecondaryNegotiator_Then_ShouldBeMarkedAsDeleted(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<ActivityUser>> activityUserRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           ActivityUser activityUserToDelete,
                       IFixture fixture)
        {
            // Arrange
            activityUserToDelete.UserType = this.GetSecondaryNegotiatorUserType(fixture);
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser> { activityUserToDelete };
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                 .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                     new[]
                                         {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                         }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            activityUserRepository.Verify(r => r.Delete(activityUserToDelete), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_AddingSecondaryNegotiator_Then_ShouldBeSaved(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           Guid secondaryNegotiatorIdToAdd,
           DateTime callDate,
           IFixture fixture)
        {
            // Arrange
            command.SecondaryNegotiators.Add(new UpdateActivityUserCommand {UserId = secondaryNegotiatorIdToAdd, CallDate = callDate });
            Activity activity = this.GetActivity(fixture);
            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                 .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                     new[]
                                         {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                         }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            ActivityUser negotiator = activity.ActivityUsers.SingleOrDefault(u => u.UserId == secondaryNegotiatorIdToAdd);
            negotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should().Be(EnumTypeItemCode.SecondaryNegotiator);
            negotiator.CallDate.Should().Be(callDate);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingSecondaryNegotiator_AndNoUserTypeChanges_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           ActivityUser activityUserToUpdate,
           Guid secondaryNegotiatorIdToUpdate,
           DateTime callDate,
           IFixture fixture)
        {
            // Arrange
            command.SecondaryNegotiators.Add(new UpdateActivityUserCommand { UserId = secondaryNegotiatorIdToUpdate, CallDate = callDate });

            activityUserToUpdate.UserId = secondaryNegotiatorIdToUpdate;
            activityUserToUpdate.UserType = this.GetSecondaryNegotiatorUserType(fixture);
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                 .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                     new[]
                                         {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                         }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            ActivityUser negotiator = activity.ActivityUsers.SingleOrDefault(u => u.UserId == secondaryNegotiatorIdToUpdate);
            negotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should().Be(EnumTypeItemCode.SecondaryNegotiator);
            negotiator.CallDate.Should().Be(callDate);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingLeadNegotiator_AndNoUserTypeChanges_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           ActivityUser activityUserToUpdate,
           Guid leadNegotiatorIdToUpdate,
           DateTime callDate,
           IFixture fixture)
        {
            // Arrange
            command.LeadNegotiator.UserId = leadNegotiatorIdToUpdate;
            command.LeadNegotiator.CallDate = callDate;

            activityUserToUpdate.UserId = leadNegotiatorIdToUpdate;
            activityUserToUpdate.UserType = this.GetLeadNegotiatorUserType(fixture);
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                 .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                     new[]
                                         {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                         }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            ActivityUser negotiator = activity.ActivityUsers.SingleOrDefault(u => u.UserId == leadNegotiatorIdToUpdate);
            negotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should().Be(EnumTypeItemCode.LeadNegotiator);
            negotiator.CallDate.Should().Be(callDate);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingSecondaryNegotiator_AndChangedToLeadNegotiator_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           ActivityUser activityUserToUpdate,
           Guid secondaryNegotiatorIdToUpdateToLead,
           DateTime callDate,
           IFixture fixture)
        {
            // Arrange
            command.LeadNegotiator.UserId = secondaryNegotiatorIdToUpdateToLead;
            command.LeadNegotiator.CallDate = callDate;

            activityUserToUpdate.UserId = secondaryNegotiatorIdToUpdateToLead;
            activityUserToUpdate.UserType = this.GetSecondaryNegotiatorUserType(fixture);
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                 .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                     new[]
                                         {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                         }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            activity.ActivityUsers
                    .Count(u => u.UserId == secondaryNegotiatorIdToUpdateToLead && u.UserType.Code == EnumTypeItemCode.SecondaryNegotiator)
                    .Should()
                    .Be(0);

            ActivityUser negotiator = activity.ActivityUsers.SingleOrDefault(u => u.UserId == secondaryNegotiatorIdToUpdateToLead);
            negotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should().Be(EnumTypeItemCode.LeadNegotiator);
            negotiator.CallDate.Should().Be(callDate);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingLeadNegotiator_AndChangedToSecondaryNegotiator_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           ActivityUser activityUserToUpdate,
           Guid leadNegotiatorIdToUpdateToSecondary,
           DateTime callDate,
           IFixture fixture)
        {
            // Arrange
            command.SecondaryNegotiators.Add(new UpdateActivityUserCommand { UserId = leadNegotiatorIdToUpdateToSecondary, CallDate = callDate });

            activityUserToUpdate.UserId = leadNegotiatorIdToUpdateToSecondary;
            activityUserToUpdate.UserType = this.GetLeadNegotiatorUserType(fixture);
            Activity activity = this.GetActivity(fixture);
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                 .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                     new[]
                                         {
                                              this.GetLeadNegotiatorUserType(fixture),
                                              this.GetSecondaryNegotiatorUserType(fixture)
                                         }.Where(expr.Compile()));

            // Act
            handler.Handle(command);

            // Assert
            ActivityUser negotiator = activity.ActivityUsers.SingleOrDefault(u => u.UserId == leadNegotiatorIdToUpdateToSecondary);
            negotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should().Be(EnumTypeItemCode.SecondaryNegotiator);
            negotiator.CallDate.Should().Be(callDate);

            activity.ActivityUsers
                    .Count(u => u.UserId == leadNegotiatorIdToUpdateToSecondary && u.UserType.Code == EnumTypeItemCode.LeadNegotiator)
                    .Should()
                    .Be(0);
        }

        private Activity GetActivity(IFixture fixture)
        {
            var activity = fixture.Create<Activity>();
            activity.ActivityUsers = new List<ActivityUser>();
            activity.Property = fixture.Create<Property>();
            activity.Property.Address = fixture.Create<Address>();
            return activity;
        }

        private EnumTypeItem GetLeadNegotiatorUserType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, EnumTypeItemCode.LeadNegotiator).Create();
        }

        private EnumTypeItem GetSecondaryNegotiatorUserType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, EnumTypeItemCode.SecondaryNegotiator).Create();
        }
    }
}