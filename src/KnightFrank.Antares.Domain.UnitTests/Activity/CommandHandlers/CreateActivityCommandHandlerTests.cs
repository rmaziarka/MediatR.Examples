namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Common;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using Moq;

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
            CreateActivityCommandHandler handler,
            CreateActivityCommand cmd,
            Guid expectedActivityId)
        {
            // Arrange 
            Activity activity = null;
            activityRepository.Setup(r => r.Add(It.IsAny<Activity>()))
                              .Returns((Activity a) =>
                              {
                                  activity = a;
                                  return activity;
                              });
            activityRepository.Setup(r => r.Save()).Callback(() => { activity.Id = expectedActivityId; });

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(cmd.ContactIds.Select(id => new Contact { Id = id }));

            // Act
            handler.Handle(cmd);

            // Assert
            activity.Should().NotBeNull();
            activity.ShouldBeEquivalentTo(cmd, opt => opt.IncludingProperties().ExcludingMissingMembers());
            activity.Id.ShouldBeEquivalentTo(expectedActivityId);
            activityRepository.Verify(r => r.Add(It.IsAny<Activity>()), Times.Once());
            activityRepository.Verify(r => r.Save(), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandHasInvalidContacts_When_Handling_Then_ShouldThrowDomainValidatorException(
            [Frozen] Mock<IDomainValidator<CreateActivityCommand>> domainValidator,
            CreateActivityCommandHandler handler,
            CreateActivityCommand cmd)
        {
            // Arrange 
            domainValidator.Setup(v => v.Validate(It.IsAny<CreateActivityCommand>()))
                           .Returns(new ValidationResult(new [] { new ValidationFailure(nameof(cmd.ContactIds), "Error") }));
                    
            // Act & Assert
            Assert.Throws<DomainValidationException>(() => { handler.Handle(cmd); });
        }
    }
}