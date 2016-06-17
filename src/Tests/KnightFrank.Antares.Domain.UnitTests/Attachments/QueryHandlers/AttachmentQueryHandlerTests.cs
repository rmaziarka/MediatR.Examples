namespace KnightFrank.Antares.Domain.UnitTests.Attachments.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Queries;
    using KnightFrank.Antares.Domain.Attachment.QueryHandlers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("AttachmentQueryHandler")]
    [Trait("FeatureTitle", "Attachment")]
    public class AttachmentQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingAttachmentId_When_Handling_Then_ShouldReturnCorrectValue(
            Guid attachmentId,
            [Frozen] Mock<IReadGenericRepository<Attachment>> attachmentRepository,
            Attachment expectedAttachment,
            IList<Attachment> attachments,
            AttachmentQuery query,
            AttachmentQueryHandler handler)
        {
            // Arrange
            query.Id = attachmentId;
            expectedAttachment.Id = attachmentId;

            attachments.Add(expectedAttachment);
            attachmentRepository.Setup(r => r.Get()).Returns(attachments.AsQueryable());

            // Act
            Attachment attachment = handler.Handle(query);

            // Assert
            Assert.NotNull(attachment);
            Assert.Equal(expectedAttachment, attachment);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingAttachmentId_When_Handling_Then_ShouldReturnNullValue(
            [Frozen] Mock<IReadGenericRepository<Attachment>> attachmentRepository,
            IList<Attachment> attachments,
            AttachmentQuery query,
            AttachmentQueryHandler handler)
        {
            // Arrange
            attachmentRepository.Setup(r => r.Get()).Returns(attachments.AsQueryable());

            // Act
            Attachment attachment = handler.Handle(query);

            // Assert
            Assert.Null(attachment);
        }
    }
}