namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityUsersMapper")]
    public class ActivityUsersMapperTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Guid leadNegotiatorTypeId = Guid.NewGuid();
        private readonly Guid secondaryNegotiatorTypeId = Guid.NewGuid();

        [Theory]
        [InlineAutoMoqData(0, 0)]
        [InlineAutoMoqData(0, 1)]
        [InlineAutoMoqData(2, 2)]
        [InlineAutoMoqData(3, 5)]
        public void Given_ValidCommand_When_Handling_Then_ShouldAssignUsers(
            int existingUsers,
            int secondaryNegotiatorsCount,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<ActivityUser>> activityUserRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityUsersMapper mapper,
            IFixture fixture)
        {
            // Arrange
            int expectedDeletesCount = existingUsers;
            
            var r = new Random();
            var command = new UpdateActivityCommand();

            command.LeadNegotiator = new UpdateActivityUser
            {
                UserId = Guid.NewGuid(),
                CallDate = DateTime.UtcNow.Date.AddDays(r.Next(1, 20))
            };

            command.SecondaryNegotiators =
                Enumerable.Range(0, secondaryNegotiatorsCount)
                          .Select(
                              i =>
                                  new UpdateActivityUser
                                  {
                                      UserId = Guid.NewGuid(),
                                      CallDate = DateTime.UtcNow.Date.AddDays(r.Next(1, 20))
                                  })
                          .ToList();

            List<UpdateActivityUser> allNegotiators = new[] { command.LeadNegotiator }.Union(command.SecondaryNegotiators).ToList();

            var activity = fixture.Create<Activity>();

            activity.ActivityUsers =
                Enumerable.Range(0, existingUsers).Select(i => fixture.Create<ActivityUser>()).ToList();

            userRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(allNegotiators.Select(x => new User { Id = x.UserId }));

            activityUserRepository.Setup(x => x.Delete(It.IsAny<ActivityUser>()))
                                  .Callback<ActivityUser>(x => activity.ActivityUsers.Remove(x));

            this.SetupEnumTypeRepository(enumTypeItemRepository, fixture);

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.ActivityUsers.Select(x => new { x.UserId, x.CallDate })
                    .Should()
                    .BeEquivalentTo(allNegotiators.Select(x => new { x.UserId, x.CallDate }));

            activityUserRepository.Verify(x=> x.Delete(It.IsAny<ActivityUser>()), Times.Exactly(expectedDeletesCount));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommandWithCallDatesInThePastButNothingChanged_When_Handling_Then_ShouldSucceed(
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityUsersMapper mapper,
            IFixture fixture)
        {
            // Arrange
            var command = new UpdateActivityCommand();
            command.LeadNegotiator = new UpdateActivityUser
            {
                UserId = Guid.NewGuid(),
                CallDate = DateTime.UtcNow.Date.AddDays(-10)
            };
            command.SecondaryNegotiators =
                Enumerable.Range(0, 2)
                          .Select(
                              i =>
                                  new UpdateActivityUser
                                  {
                                      UserId = Guid.NewGuid(),
                                      CallDate = DateTime.UtcNow.Date.AddDays(-10)
                                  })
                          .ToList();
            List<UpdateActivityUser> allNegotiators = new[] { command.LeadNegotiator }.Union(command.SecondaryNegotiators).ToList();

            List<ActivityUser> originalUsers =
                command.SecondaryNegotiators.Select(
                    x => new ActivityUser { UserId = x.UserId, CallDate = x.CallDate, UserTypeId = this.secondaryNegotiatorTypeId })
                       .ToList();
            originalUsers.Add(new ActivityUser
            {
                UserId = command.LeadNegotiator.UserId,
                CallDate = command.LeadNegotiator.CallDate,
                UserTypeId = this.leadNegotiatorTypeId
            });
            var activity = fixture.Create<Activity>();
            activity.ActivityUsers = originalUsers;

            userRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(allNegotiators.Select(x => new User { Id = x.UserId }));
            this.SetupEnumTypeRepository(enumTypeItemRepository, fixture);

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.ActivityUsers.Should().BeEquivalentTo(originalUsers);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandWithAnyCallDateInThePastThatChanged_When_Handling_Then_ShouldThrowException(
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityUsersMapper mapper,
            IFixture fixture)
        {
            // Arrange
            var command = new UpdateActivityCommand();
            command.LeadNegotiator = new UpdateActivityUser
            {
                UserId = Guid.NewGuid(),
                CallDate = DateTime.Today.Date.AddDays(-10)
            };

            var activity = fixture.Create<Activity>();
            activity.ActivityUsers = new List<ActivityUser> {new ActivityUser {UserId = command.LeadNegotiator.UserId, CallDate = DateTime.Today } };

            userRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(new []{ new User { Id = command.LeadNegotiator.UserId } });
            this.SetupEnumTypeRepository(enumTypeItemRepository, fixture);

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => mapper.ValidateAndAssign(command, activity));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandWithNewUserWithCallDateInThePast_When_Handling_Then_ShouldThrowException(
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityUsersMapper mapper,
            IFixture fixture)
        {
            // Arrange
            var command = new UpdateActivityCommand();
            command.LeadNegotiator = new UpdateActivityUser
            {
                UserId = Guid.NewGuid(),
                CallDate = DateTime.Today.Date.AddDays(-10)
            };

            var activity = fixture.Create<Activity>();
            activity.ActivityUsers = new List<ActivityUser>();

            userRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(new[] { new User { Id = command.LeadNegotiator.UserId } });
            this.SetupEnumTypeRepository(enumTypeItemRepository, fixture);

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => mapper.ValidateAndAssign(command, activity));
        }

        private void SetupEnumTypeRepository(Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository, IFixture fixture)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                      {
                                          this.GetLeadNegotiatorUserType(fixture),
                                          this.GetSecondaryNegotiatorUserType(fixture)
                                      }.Where(expr.Compile()));
        }

        private EnumTypeItem GetLeadNegotiatorUserType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>()
                          .With(i => i.Id, this.leadNegotiatorTypeId)
                          .With(i => i.Code, ActivityUserType.LeadNegotiator.ToString()).Create();
        }

        private EnumTypeItem GetSecondaryNegotiatorUserType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>()
                          .With(i => i.Id, this.secondaryNegotiatorTypeId)
                          .With(i => i.Code, ActivityUserType.SecondaryNegotiator.ToString()).Create();
        }
    }
}
