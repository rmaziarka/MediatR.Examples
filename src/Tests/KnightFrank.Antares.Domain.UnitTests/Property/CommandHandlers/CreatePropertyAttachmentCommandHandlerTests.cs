namespace KnightFrank.Antares.Domain.UnitTests.Property.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Property.CommandHandlers;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Property Attachment")]
    [Collection("CreatePropertyAttachmentCommandHandler")]
    public class CreatePropertyAttachmentCommandHandlerTests : BaseTestClassFixture
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldSaveAttachment(
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            CreatePropertyAttachmentCommandHandler handler,
            CreateEntityAttachmentCommand cmd,
            Property property,
            Guid expectedAttachmentId)
        {
            // Arrange
            property.Attachments = new List<Attachment>();
            propertyRepository.Setup(r => r.GetById(cmd.EntityId)).Returns(property);

            propertyRepository.Setup(x => x.Save()).Callback(() =>
            {
                property.Attachments.Single(x => x.FileName == cmd.Attachment.FileName).Id = expectedAttachmentId;
            });

            // Act
            Guid attachmentId = handler.Handle(cmd);
            
            // Assert
            Assert.Equal(expectedAttachmentId, attachmentId);

            entityValidator.Verify(x => x.EntityExists(property, cmd.EntityId));
            entityValidator.Verify(x => x.EntityExists<User>(cmd.Attachment.UserId));

            enumTypeItemValidator.Verify(x => x.ItemExists(EnumType.PropertyDocumentType, cmd.Attachment.DocumentTypeId));

            Attachment attachment = property.Attachments.SingleOrDefault();
            Assert.NotNull(attachment);
            cmd.Attachment.ShouldBeEquivalentTo(attachment, opt => opt.IncludingProperties());
        }
    }
}
