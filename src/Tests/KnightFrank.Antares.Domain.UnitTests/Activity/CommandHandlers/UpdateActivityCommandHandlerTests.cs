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
    using Ploeh.AutoFixture.Dsl;
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
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            UpdateActivityCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            UpdateActivityCommand command = this.CreateUpdateActivityCommand(fixture, DateTime.Today);
            Activity activity = this.GetActivity(fixture);

            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            // Act
            handler.Handle(command);

            // Assert
            this.VerifyIfValidationsWereCalled(activity, command, entityValidator, collectionValidator);

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
        public void Given_ValidCommandWithCallDatesInThePastButNothingChanged_When_Handling_Then_ShouldUpdateActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            ActivityUser activityLeadNegotiator,
            List<ActivityUser> activitySecondaryNegotiators,
            UpdateActivityCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            var dateInThePast = new DateTime(2000, 1, 1);

            UpdateActivityCommand command = this.CreateUpdateActivityCommand(fixture, null);
            Activity activity = this.GetActivity(fixture, activityLeadNegotiator, activitySecondaryNegotiators);

            var rnd = new Random();
            activity.ActivityUsers.ToList().ForEach(u => u.CallDate = dateInThePast.AddDays(rnd.Next(1, 15)));

            command.LeadNegotiator = this.CreateUpdateActivityUser(fixture, activityLeadNegotiator.UserId, activityLeadNegotiator.CallDate).Create();
            command.SecondaryNegotiators[0] = this.CreateUpdateActivityUser(fixture, activitySecondaryNegotiators[0].UserId, activitySecondaryNegotiators[0].CallDate).Create();
            command.SecondaryNegotiators[1] = this.CreateUpdateActivityUser(fixture, activitySecondaryNegotiators[1].UserId, activitySecondaryNegotiators[1].CallDate).Create();

            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            // Act
            Action act = () => handler.Handle(command);

            // Assert
            act.ShouldNotThrow<BusinessValidationException>();
        }

        [Theory]
        [InlineAutoMoqData(true, 0, true)]
        [InlineAutoMoqData(true, 1, true)]
        [InlineAutoMoqData(true, 2, true)]
        [InlineAutoMoqData(true, 0, false)]
        [InlineAutoMoqData(true, 1, false)]
        [InlineAutoMoqData(true, 2, false)]
        [InlineAutoMoqData(false, 0, true)]
        [InlineAutoMoqData(false, 1, true)]
        [InlineAutoMoqData(false, 2, true)]
        [InlineAutoMoqData(false, 0, false)]
        [InlineAutoMoqData(false, 1, true)]
        [InlineAutoMoqData(false, 2, true)]
        public void Given_CommandWithAnyCallDateInThePastThatChanged_When_Handling_Then_ShouldThrowException(
            bool isLeadNegotiatorValid,
            int invalidSecondaryNegotiatorIndex,
            bool areOtherDatesInFuture,
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            ActivityUser activityLeadNegotiator,
            List<ActivityUser> activitySecondaryNegotiators,
            UpdateActivityCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            DateTime dateInTheFuture = DateTime.Today.AddDays(1);
            var dateInThePast = new DateTime(2000, 1, 1);
            var dateInThePastOther = new DateTime(1999, 1, 1);

            UpdateActivityCommand command = this.CreateUpdateActivityCommand(fixture, areOtherDatesInFuture ? dateInTheFuture : dateInThePast);
            Activity activity = this.GetActivity(fixture, activityLeadNegotiator, activitySecondaryNegotiators);

            var rnd = new Random();
            activity.ActivityUsers.ToList().ForEach(u => u.CallDate = dateInThePast.AddDays(rnd.Next(1, 15)));

            command.LeadNegotiator = this.CreateUpdateActivityUser(fixture, activityLeadNegotiator.UserId, activityLeadNegotiator.CallDate).Create();
            command.SecondaryNegotiators[0] = this.CreateUpdateActivityUser(fixture, activitySecondaryNegotiators[0].UserId, activitySecondaryNegotiators[0].CallDate).Create();
            command.SecondaryNegotiators[1] = this.CreateUpdateActivityUser(fixture, activitySecondaryNegotiators[1].UserId, activitySecondaryNegotiators[1].CallDate).Create();

            if (!isLeadNegotiatorValid)
            {
                // ReSharper disable once PossibleInvalidOperationException
                command.LeadNegotiator.CallDate = dateInThePastOther;
            }

            if (invalidSecondaryNegotiatorIndex != -1)
            {
                // ReSharper disable once PossibleInvalidOperationException
                command.SecondaryNegotiators[invalidSecondaryNegotiatorIndex].CallDate = dateInThePastOther;
            }

            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            // Act
            Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_DeletingSecondaryNegotiator_Then_ShouldBeMarkedAsDeleted(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<ActivityUser>> activityUserRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           ActivityUser activityLeadNegotiator,
           UpdateActivityCommandHandler handler,
           ActivityUser secondaryNegotiatorToDelete,
           IFixture fixture)
        {
            // Arrange
            UpdateActivityCommand command = this.CreateUpdateActivityCommand(fixture, DateTime.Today);
            Activity activity = this.GetActivity(fixture, activityLeadNegotiator);

            secondaryNegotiatorToDelete.UserType = this.GetSecondaryNegotiatorUserType(fixture);
            activity.ActivityUsers.Add(secondaryNegotiatorToDelete);

            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            // Act
            handler.Handle(command);

            // Assert
            activityUserRepository.Verify(r => r.Delete(secondaryNegotiatorToDelete), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_AddingSecondaryNegotiator_Then_ShouldBeSaved(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           ActivityUser activityLeadNegotiator,
           UpdateActivityCommandHandler handler,
           Guid secondaryNegotiatorIdToAdd,
           IFixture fixture)
        {
            // Arrange
            UpdateActivityCommand command = this.CreateUpdateActivityCommand(fixture, DateTime.Today);
            Activity activity = this.GetActivity(fixture, activityLeadNegotiator);

            DateTime callDate = DateTime.Today.AddDays(1);
            command.SecondaryNegotiators.Add(new UpdateActivityUser { UserId = secondaryNegotiatorIdToAdd, CallDate = callDate });

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

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
        [InlineAutoMoqData(false, true)]
        [InlineAutoMoqData(false, false)]
        [InlineAutoMoqData(true, true)]
        [InlineAutoMoqData(true, false)]
        public void Given_ValidCommand_When_UpdatingNegotiator_Then_ShouldBeUpdated_WithPropertUserType(
           bool isLeadNegotiatorToBeChanged,
           bool isNegotiatorTypeToChange,
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           ActivityUser activityLeadNegotiator,
           List<ActivityUser> activitySecondaryNegotiators,
           UpdateActivityCommandHandler handler,
           Guid negotiatorIdToUpdate,
           IFixture fixture)
        {
            // Arrange
            bool isNegotiatorToBeChangedToLead = (isLeadNegotiatorToBeChanged && !isNegotiatorTypeToChange) ||
                                                 (!isLeadNegotiatorToBeChanged && isNegotiatorTypeToChange);

            UpdateActivityCommand command = this.CreateUpdateActivityCommand(fixture, DateTime.Today);
            Activity activity = this.GetActivity(fixture, activityLeadNegotiator, activitySecondaryNegotiators);

            DateTime callDate = DateTime.Today.AddDays(1);
            if (isNegotiatorToBeChangedToLead)
            {
                command.LeadNegotiator = this.CreateUpdateActivityUser(fixture, negotiatorIdToUpdate, callDate).Create();
            }
            else
            {
                command.SecondaryNegotiators.Add(this.CreateUpdateActivityUser(fixture, negotiatorIdToUpdate, callDate).Create());
            }

            if (isLeadNegotiatorToBeChanged)
            {
                activityLeadNegotiator.UserId = negotiatorIdToUpdate;
            }
            else
            {
                activitySecondaryNegotiators[1].UserId = negotiatorIdToUpdate;
            }

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            // Act
            handler.Handle(command);

            // Assert
            ActivityUser negotiator = activity.ActivityUsers.SingleOrDefault(u => u.UserId == negotiatorIdToUpdate);
            negotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should().Be(isNegotiatorToBeChangedToLead ? EnumTypeItemCode.LeadNegotiator: EnumTypeItemCode.SecondaryNegotiator);
            negotiator.CallDate.Should().Be(callDate);
        }

        private Activity GetActivity(IFixture fixture, ActivityUser activityLeadNegotiator = null, List<ActivityUser> activitySecondaryNegotiators = null)
        {
            var activity = fixture.Create<Activity>();
            activity.Property = fixture.Create<Property>();
            activity.Property.Address = fixture.Create<Address>();

            if (activityLeadNegotiator == null)
            {
                activityLeadNegotiator = fixture.Build<ActivityUser>().With(i => i.UserType, this.GetLeadNegotiatorUserType(fixture)).Create();
            }
            else
            {
                activityLeadNegotiator.UserType = this.GetLeadNegotiatorUserType(fixture);
            }

            activity.ActivityUsers = new List<ActivityUser> { activityLeadNegotiator };

            activitySecondaryNegotiators?.ForEach(negotiator =>
            {
                negotiator.UserType = this.GetSecondaryNegotiatorUserType(fixture);
                activity.ActivityUsers.Add(negotiator);
            });

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

        private IPostprocessComposer<UpdateActivityUser> CreateUpdateActivityUser(IFixture fixture, Guid userId, DateTime? callDate)
        {
            return fixture.Build<UpdateActivityUser>().With(i => i.UserId, userId).With(i => i.CallDate, callDate);
        }

        private UpdateActivityCommand CreateUpdateActivityCommand(IFixture fixture, DateTime? baseCallDate)
        {
            var rnd = new Random();
            return fixture.Build<UpdateActivityCommand>()
                          .With(i => i.LeadNegotiator, this.CreateUpdateActivityUser(fixture, fixture.Create<Guid>(), baseCallDate).Create())
                          .With(i => i.SecondaryNegotiators, this.CreateUpdateActivityUser(fixture, fixture.Create<Guid>(), baseCallDate?.AddDays(rnd.Next(1, 15))).CreateMany().ToList())
                          .Create();
        }

        private void SetupEnumTypeItemRepository(Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository, IFixture fixture)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                      {
                                          this.GetLeadNegotiatorUserType(fixture),
                                          this.GetSecondaryNegotiatorUserType(fixture)
                                      }.Where(expr.Compile()));
        }

        private void VerifyIfValidationsWereCalled(Activity activity, UpdateActivityCommand command, Mock<IEntityValidator> entityValidator, Mock<ICollectionValidator> collectionValidator)
        {
            entityValidator.Verify(x => x.EntityExists(activity, activity.Id), Times.Once);
            entityValidator.Verify(x => x.EntityExists<User>(command.LeadNegotiator.UserId), Times.Once);
            entityValidator.Verify(x => x.EntitiesExist<User>(It.Is<List<Guid>>(list => list.SequenceEqual(command.SecondaryNegotiators.Select(n => n.UserId).ToList()))), Times.Once);

            List<Guid> expectedNegotiators = command.SecondaryNegotiators.Select(n => n.UserId).ToList();
            expectedNegotiators.Add(command.LeadNegotiator.UserId);

            collectionValidator.Verify(
                x => x.CollectionIsUnique(
                        It.Is<ICollection<Guid>>(list => list.OrderBy(i => i).SequenceEqual(expectedNegotiators.OrderBy(i => i))),
                        ErrorMessage.Activity_Negotiators_Not_Unique), Times.Once);

        }
    }
}