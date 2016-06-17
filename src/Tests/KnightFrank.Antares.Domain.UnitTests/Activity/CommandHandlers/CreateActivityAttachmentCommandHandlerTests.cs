namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity Attachment")]
    [Collection("CreateActivityAttachmentCommandHandler")]
    public class CreateActivityAttachmentCommandHandlerTests : BaseTestClassFixture
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldSaveAttachment(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            CreateActivityAttachmentCommandHandler handler,
            CreateActivityAttachmentCommand cmd,
            Activity activity)
        {
            // Arrange
            activity.Attachments = new List<Attachment>();
            activityRepository.Setup(r => r.GetById(cmd.ActivityId)).Returns(activity);

            // Act
            handler.Handle(cmd);
            Attachment attachment = activity.Attachments.SingleOrDefault();

            // Assert
            Assert.NotNull(attachment);
            cmd.Attachment.ShouldBeEquivalentTo(attachment, opt => opt.IncludingProperties());
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldValidateActivityDocumentTypeItem(
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            CreateActivityAttachmentCommandHandler handler,
            CreateActivityAttachmentCommand cmd,
            Activity activity)
        {
            // Arrange
            activity.Attachments = new List<Attachment>();
            activityRepository.Setup(r => r.GetById(cmd.ActivityId)).Returns(activity);

            // Act
            handler.Handle(cmd);

            // Assert
            enumTypeItemValidator.Verify(x => x.ItemExists(EnumType.ActivityDocumentType, cmd.Attachment.DocumentTypeId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldValidateUserId(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            CreateActivityAttachmentCommandHandler handler,
            CreateActivityAttachmentCommand cmd,
            Activity activity)
        {
            // Arrange
            activity.Attachments = new List<Attachment>();
            activityRepository.Setup(r => r.GetById(cmd.ActivityId)).Returns(activity);

            // Act
            handler.Handle(cmd);

            // Assert
            entityValidator.Verify(x => x.EntityExists<User>(cmd.Attachment.UserId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldValidateActivityId(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            CreateActivityAttachmentCommandHandler handler,
            CreateActivityAttachmentCommand cmd,
            Activity activity)
        {
            // Arrange
            activity.Attachments = new List<Attachment>();
            activityRepository.Setup(r => r.GetById(cmd.ActivityId)).Returns(activity);

            // Act
            handler.Handle(cmd);

            // Assert
            entityValidator.Verify(x => x.EntityExists(activity, cmd.ActivityId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldReturnCreatedAttachmentId(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            CreateActivityAttachmentCommandHandler handler,
            CreateActivityAttachmentCommand cmd,
            Activity activity,
            Guid expectedAttachmentId)
        {
            // Arrange
            activity.Attachments = new List<Attachment>();
            activityRepository.Setup(r => r.GetById(cmd.ActivityId)).Returns(activity);

            activityRepository.Setup(x => x.Save()).Callback(() =>
            {
                Attachment attachment = activity.Attachments.SingleOrDefault();
                if (attachment != null)
                {
                    attachment.Id = expectedAttachmentId;
                }
            });

            // Act
            Guid attachmentId = handler.Handle(cmd);

            // Assert
            Assert.Equal(expectedAttachmentId, attachmentId);
        }
    }
}