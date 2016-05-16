namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Moq;

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
            UpdateActivityCommand command,
            UpdateActivityCommandHandler handler,
            Activity activity)
        {
            activity.ActivityUsers = new List<ActivityUser>();
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            handler.Handle(command);

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
        public void Given_ValidCommand_When_DeletingSecondaryNegotiator_Then_ShouldBeMarkedAsDeleted(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           [Frozen] Mock<IGenericRepository<ActivityUser>> activityUserRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           Activity activity,
           ActivityUser activityUserToDelete)
        {
            // Arrange
            activityUserToDelete.UserType = UserTypeEnum.SecondaryNegotiator;
            activity.ActivityUsers = new List<ActivityUser> { activityUserToDelete };
            activityRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            // Act
            handler.Handle(command);

            // Assert
            activityUserRepository.Verify(r => r.Delete(activityUserToDelete), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_AddingSecondaryNegotiator_Then_ShouldBeSaved(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           Activity activity,
           Guid secondaryNegotiatorIdToAdd)
        {
            // Arrange
            command.SecondaryNegotiatorIds.Add(secondaryNegotiatorIdToAdd);
            activity.ActivityUsers = new List<ActivityUser>();
            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            // Act
            handler.Handle(command);

            // Assert
            activity.ActivityUsers
                    .Count(u => u.UserId == secondaryNegotiatorIdToAdd && u.UserType == UserTypeEnum.SecondaryNegotiator)
                    .Should()
                    .Be(1);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingSecondaryNegotiator_AndNoUserTypeChanges_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           Activity activity,
           ActivityUser activityUserToUpdate,
           Guid secondaryNegotiatorIdToUpdate)
        {
            // Arrange
            command.SecondaryNegotiatorIds.Add(secondaryNegotiatorIdToUpdate);

            activityUserToUpdate.UserId = secondaryNegotiatorIdToUpdate;
            activityUserToUpdate.UserType = UserTypeEnum.SecondaryNegotiator;
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            // Act
            handler.Handle(command);

            // Assert
            activity.ActivityUsers
                    .Count(u => u.UserId == secondaryNegotiatorIdToUpdate && u.UserType == UserTypeEnum.SecondaryNegotiator)
                    .Should()
                    .Be(1);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingLeadNegotiator_AndNoUserTypeChanges_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           Activity activity,
           ActivityUser activityUserToUpdate,
           Guid leadNegotiatorIdToUpdate)
        {
            // Arrange
            command.LeadNegotiatorId = leadNegotiatorIdToUpdate;

            activityUserToUpdate.UserId = leadNegotiatorIdToUpdate;
            activityUserToUpdate.UserType = UserTypeEnum.LeadNegotiator;
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            // Act
            handler.Handle(command);

            // Assert
            activity.ActivityUsers
                    .Count(u => u.UserId == leadNegotiatorIdToUpdate && u.UserType == UserTypeEnum.LeadNegotiator)
                    .Should()
                    .Be(1);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingSecondaryNegotiator_AndChangedToLeadNegotiator_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           Activity activity,
           ActivityUser activityUserToUpdate,
           Guid secondaryNegotiatorIdToUpdateToLead)
        {
            // Arrange
            command.LeadNegotiatorId = secondaryNegotiatorIdToUpdateToLead;

            activityUserToUpdate.UserId = secondaryNegotiatorIdToUpdateToLead;
            activityUserToUpdate.UserType = UserTypeEnum.SecondaryNegotiator;
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            // Act
            handler.Handle(command);

            // Assert
            activity.ActivityUsers
                    .Count(u => u.UserId == secondaryNegotiatorIdToUpdateToLead && u.UserType == UserTypeEnum.SecondaryNegotiator)
                    .Should()
                    .Be(0);

            activity.ActivityUsers
                    .Count(u => u.UserId == secondaryNegotiatorIdToUpdateToLead && u.UserType == UserTypeEnum.LeadNegotiator)
                    .Should()
                    .Be(1);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_UpdatingLeadNegotiator_AndChangedToSecondaryNegotiator_Then_ShouldBeUpdated(
           [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
           UpdateActivityCommand command,
           UpdateActivityCommandHandler handler,
           Activity activity,
           ActivityUser activityUserToUpdate,
           Guid leadNegotiatorIdToUpdateToSecondary)
        {
            // Arrange
            command.SecondaryNegotiatorIds.Add(leadNegotiatorIdToUpdateToSecondary);

            activityUserToUpdate.UserId = leadNegotiatorIdToUpdateToSecondary;
            activityUserToUpdate.UserType = UserTypeEnum.LeadNegotiator;
            activity.ActivityUsers = new List<ActivityUser> { activityUserToUpdate };

            activityRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<Expression<Func<Activity, object>>[]>())).Returns(new List<Activity> { activity });

            // Act
            handler.Handle(command);

            // Assert
            activity.ActivityUsers
                    .Count(u => u.UserId == leadNegotiatorIdToUpdateToSecondary && u.UserType == UserTypeEnum.SecondaryNegotiator)
                    .Should()
                    .Be(1);

            activity.ActivityUsers
                    .Count(u => u.UserId == leadNegotiatorIdToUpdateToSecondary && u.UserType == UserTypeEnum.LeadNegotiator)
                    .Should()
                    .Be(0);
        }
    }
}