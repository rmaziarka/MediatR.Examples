namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

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
            User user,
            CreateActivityCommandHandler handler,
            CreateActivityCommand command,
            Guid expectedActivityId)
        {
            // Arrange
            Activity activity = null;

            userRepository.Setup(p => p.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(new List<User> { user });

            activityRepository.Setup(r => r.Add(It.IsAny<Activity>()))
                              .Returns((Activity a) =>
                              {
                                  activity = a;
                                  return activity;
                              });
            activityRepository.Setup(r => r.Save()).Callback(() => { activity.Id = expectedActivityId; });

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(command.ContactIds.Select(id => new Contact { Id = id }));

            // Act
            handler.Handle(command);

            // Assert
            activity.Should().NotBeNull();
            activity.ShouldBeEquivalentTo(command, opt => opt.IncludingProperties().ExcludingMissingMembers());
            activity.Id.ShouldBeEquivalentTo(expectedActivityId);
            activityRepository.Verify(r => r.Add(It.IsAny<Activity>()), Times.Once());
            activityRepository.Verify(r => r.Save(), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_Command_When_Handling_Then_EntityExistsValidation_ShouldBeCalledForNegotiator(
            [Frozen] Mock<IDomainValidator<CreateActivityCommand>> domainValidator,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            User user,
            CreateActivityCommandHandler handler,
            CreateActivityCommand command,
            IFixture fixture
            )
        {
            // Arrange
            user.Id = command.LeadNegotiatorId;
            userRepository.Setup(p => p.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(new List<User> { user });

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                 .Returns(command.ContactIds.Select(id => new Contact { Id = id }));

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists(user, user.Id), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateActivityCommandWithInvalidVendors_When_Handling_Then_ShouldThrowBusinessException(
            [Frozen] Mock<IDomainValidator<CreateActivityCommand>> domainValidator,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            User user,
            CreateActivityCommandHandler handler,
            CreateActivityCommand command,
            IFixture fixture
            )
        {
            // Arrange
            userRepository.Setup(p => p.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                          .Returns(new List<User> { user });

            command.ContactIds = fixture.CreateMany<Guid>(2).ToList();

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(
                new List<Contact>()
                {
                    fixture.Build<Contact>().With(x => x.Id, command.ContactIds.First()).Create()
                });

            // Act + Assert
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });
            Assert.Equal(ErrorMessage.Missing_Activity_Vendors_Id, businessValidationException.ErrorCode);
        }
    }
}