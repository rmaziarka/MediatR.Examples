namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
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
    [Collection("UpdateActivityUserCommandHandler")]
    public class UpdateActivityUserCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        public static IEnumerable<object[]> FixtureAllCombination = new[]
                                                                        {
                                                                            new object[] { true, DateTime.Now.AddDays(1) },
                                                                            new object[] { true, null },
                                                                            new object[] { false, DateTime.Now.AddDays(1) },
                                                                            new object[] { false, null }
                                                                        };

        public static IEnumerable<object[]> FixtureCorrectCombination = new[]
                                                                            {
                                                                                new object[] { true, DateTime.Now.AddDays(1) },
                                                                                new object[] { false, DateTime.Now.AddDays(1) },
                                                                                new object[] { false, null }
                                                                            };

        [Theory]
        [MemberAutoMoqData("FixtureCorrectCombination", MemberType = typeof(UpdateActivityUserCommandHandlerTests))]
        public void Given_ValidCommand_When_Handling_Then_ShouldUpdateActivity(
            bool isLeadNegotiator,
            DateTime? callDate,
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<ActivityUser>> activityUserRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            UpdateActivityUserCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            UpdateActivityUserCommand command = this.CreateUpdateActivityUserCommand(callDate, fixture);
            Activity activity = this.CreateActivity(command.ActivityId, fixture);
            activityRepository.Setup(r => r.GetById(command.ActivityId)).Returns(activity);
            ActivityUser activityUser = this.CreateActivityUser(command.Id, command.ActivityId, isLeadNegotiator, fixture);
            activityUserRepository.Setup(
                r =>
                r.GetWithInclude(
                    It.IsAny<Expression<Func<ActivityUser, bool>>>(),
                    It.IsAny<Expression<Func<ActivityUser, object>>[]>()))
                                  .Returns(
                                      (Expression<Func<ActivityUser, bool>> exp1, Expression<Func<ActivityUser, object>>[] exp2) =>
                                      new[] { activityUser }.Where(exp1.Compile()));

            DateTime? updatedCallDate = DateTime.MinValue;
            activityUserRepository.Setup(r => r.Save())
                                  .Callback(
                                      () =>
                                          {
                                              updatedCallDate = activityUser.CallDate.HasValue
                                                                    ? new DateTime?(activityUser.CallDate.Value)
                                                                    : null;
                                          });

            // Act
            Guid activityUserId = handler.Handle(command);

            // Assert
            activityUserId.Should().Be(command.Id);

            entityValidator.Verify(x => x.EntityExists(activity, command.ActivityId), Times.Once);
            entityValidator.Verify(x => x.EntityExists(activityUser, command.Id), Times.Once);

            activityUserRepository.Verify(x => x.Save(), Times.Once);
            updatedCallDate.Should().Be(command.CallDate);
        }

        [Theory]
        [MemberAutoMoqData("FixtureAllCombination", MemberType = typeof(UpdateActivityUserCommandHandlerTests))]
        public void Given_ActivityUserAssignToOtherActivity_When_Handling_Then_ShouldThrowException(
            bool isLeadNegotiator,
            DateTime? callDate,
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<ActivityUser>> activityUserRepository,
            UpdateActivityUserCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            UpdateActivityUserCommand command = this.CreateUpdateActivityUserCommand(callDate, fixture);
            Activity activity = this.CreateActivity(command.ActivityId, fixture);
            activityRepository.Setup(r => r.GetById(command.ActivityId)).Returns(activity);
            ActivityUser activityUser = this.CreateActivityUser(command.Id, Guid.NewGuid(), isLeadNegotiator, fixture);
            activityUserRepository.Setup(
                r =>
                r.GetWithInclude(
                    It.IsAny<Expression<Func<ActivityUser, bool>>>(),
                    It.IsAny<Expression<Func<ActivityUser, object>>[]>()))
                                  .Returns(
                                      (Expression<Func<ActivityUser, bool>> exp1, Expression<Func<ActivityUser, object>>[] exp2) =>
                                      new[] { activityUser }.Where(exp1.Compile()));

            // Act
            Action act = () => handler.Handle(command);

            // Asset
            act.ShouldThrow<BusinessValidationException>()
               .Which.ErrorCode.ShouldBeEquivalentTo(ErrorMessage.ActivityUser_Is_Assigned_To_Other_Activity);
        }

        [Theory]
        [AutoMoqData]
        public void Given_LeadActivityUserAndNotSpecifyCallDate_When_Handling_Then_ShouldThrowException(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen] Mock<IGenericRepository<ActivityUser>> activityUserRepository,
            UpdateActivityUserCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            UpdateActivityUserCommand command = this.CreateUpdateActivityUserCommand(null, fixture);
            Activity activity = this.CreateActivity(command.ActivityId, fixture);
            activityRepository.Setup(r => r.GetById(command.ActivityId)).Returns(activity);
            ActivityUser activityUser = this.CreateActivityUser(command.Id, command.ActivityId, true, fixture);
            activityUserRepository.Setup(
                r =>
                r.GetWithInclude(
                    It.IsAny<Expression<Func<ActivityUser, bool>>>(),
                    It.IsAny<Expression<Func<ActivityUser, object>>[]>()))
                                  .Returns(
                                      (Expression<Func<ActivityUser, bool>> exp1, Expression<Func<ActivityUser, object>>[] exp2) =>
                                      new[] { activityUser }.Where(exp1.Compile()));

            // Act
            Action act = () => handler.Handle(command);

            // Asset
            act.ShouldThrow<BusinessValidationException>()
               .Which.ErrorCode.ShouldBeEquivalentTo(ErrorMessage.ActivityUser_CallDate_Is_Required);
        }

        private UpdateActivityUserCommand CreateUpdateActivityUserCommand(DateTime? callDate, IFixture fixture)
        {
            return fixture.Build<UpdateActivityUserCommand>().With(c => c.CallDate, callDate).Create();
        }

        private Activity CreateActivity(Guid activityId, IFixture fixture)
        {
            return fixture.Build<Activity>().With(a => a.Id, activityId).Create();
        }

        private ActivityUser CreateActivityUser(Guid activityUserId, Guid activityId, IFixture fixture)
        {
            return fixture.Build<ActivityUser>().With(a => a.Id, activityUserId).With(a => a.ActivityId, activityId).Create();
        }

        private ActivityUser CreateActivityUser(Guid activityUserId, Guid activityId, bool isLeadNegotiator, IFixture fixture)
        {
            string userTypeCode = isLeadNegotiator ? EnumTypeItemCode.LeadNegotiator : EnumTypeItemCode.SecondaryNegotiator;
            EnumTypeItem userType = fixture.Build<EnumTypeItem>().With(i => i.Code, userTypeCode).Create();

            return
                fixture.Build<ActivityUser>()
                       .With(a => a.Id, activityUserId)
                       //.With(a => a.UserId, activityUserId)
                       .With(a => a.ActivityId, activityId)
                       .With(a => a.UserType, userType)
                       .Create();
        }
    }
}
