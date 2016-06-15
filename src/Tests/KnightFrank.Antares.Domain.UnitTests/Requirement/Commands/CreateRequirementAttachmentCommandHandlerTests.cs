namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Requirement.CommandHandlers;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Requirement Attachment")]
    [Collection("CreateRequirementAttachmentCommandHandler")]
    public class CreateRequirementAttachmentCommandHandlerTests : BaseTestClassFixture
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldSaveAttachment(
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            CreateRequirementAttachmentCommandHandler handler,
            CreateRequirementAttachmentCommand cmd,
            Requirement requirement,
            Guid expectedAttachmentId)
        {
            // Arrange
            requirement.Attachments = new List<Attachment>();
            requirementRepository.Setup(r => r.GetById(cmd.EntityId)).Returns(requirement);

            requirementRepository.Setup(x => x.Save()).Callback(() =>
            {
                requirement.Attachments.Single(x => x.FileName == cmd.Attachment.FileName).Id = expectedAttachmentId;
            });

            // Act
            Guid attachmentId = handler.Handle(cmd);

            // Assert
            Assert.Equal(expectedAttachmentId, attachmentId);

            entityValidator.Verify(x => x.EntityExists(requirement, cmd.EntityId));
            entityValidator.Verify(x => x.EntityExists<User>(cmd.Attachment.UserId));

            enumTypeItemValidator.Verify(x => x.ItemExists(EnumType.RequirementDocumentType, cmd.Attachment.DocumentTypeId));

            Attachment attachment = requirement.Attachments.SingleOrDefault();
            Assert.NotNull(attachment);
            cmd.Attachment.ShouldBeEquivalentTo(attachment, opt => opt.IncludingProperties());
        }
    }
}