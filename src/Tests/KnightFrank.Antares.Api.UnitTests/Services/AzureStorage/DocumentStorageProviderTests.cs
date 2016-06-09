namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class DocumentStorageProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_GetDocumentUploadSasUriForActivity_Then_ShouldDelegateBehaviour(
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            DocumentType documentType,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            DocumentStorageProvider documentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = documentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            documentStorageProvider.GetUploadSasUri<Activity>(parameters, EnumType.ActivityDocumentType, CloudStorageContainerType.Activity);

            // Assert
            blobResourceFactory.Verify(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity), Times.Once);
            storageClient.Verify(x => x.GetSasUri(resource, policy), Times.Once);
            enumTypeItemValidator.Verify(x => x.ItemExists(enumTypeItem, parameters.DocumentTypeId), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Activity>(parameters.EntityReferenceId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetDocumentDownloadSasUriForActivity_Then_ShouldDelegateBehaviour(
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            DocumentType documentType,
            ICloudBlobResource resource,
            AttachmentDownloadUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            DocumentStorageProvider documentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = documentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            documentStorageProvider.GetDownloadSasUri<Activity>(parameters, EnumType.ActivityDocumentType, CloudStorageContainerType.Activity);

            // Assert
            blobResourceFactory.Verify(x => x.Create(documentType, parameters.ExternalDocumentId, parameters, CloudStorageContainerType.Activity), Times.Once);
            storageClient.Verify(x => x.GetSasUri(resource, policy), Times.Once);
            enumTypeItemValidator.Verify(x => x.ItemExists(enumTypeItem, parameters.DocumentTypeId), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Activity>(parameters.EntityReferenceId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetDocumentUploadSasUri_Then_ShouldReturnCreatedUrl(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            DocumentType documentType,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            DocumentStorageProvider documentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = documentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            AzureUploadUrlContainer azureUploadUrlContainer = documentStorageProvider.GetUploadSasUri<Activity>(parameters, EnumType.ActivityDocumentType, CloudStorageContainerType.Activity);

            // Assert
            Assert.Equal(uri, azureUploadUrlContainer.Url);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetDocumentDownloadSasUri_Then_ShouldReturnCreatedUrl(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            DocumentType documentType,
            ICloudBlobResource resource,
            AttachmentDownloadUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            DocumentStorageProvider documentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = documentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            AzureDownloadUrlContainer container = documentStorageProvider.GetDownloadSasUri<Activity>(parameters, EnumType.ActivityDocumentType, CloudStorageContainerType.Activity);

            // Assert
            Assert.Equal(uri, container.Url);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetDocumentUploadSasUri_Then_ShouldReturnCreatedExternalId(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            DocumentType documentType,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            DocumentStorageProvider documentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = documentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            Guid externalDocumentId = Guid.Empty;

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity))
                .Callback((DocumentType docType, Guid documentId, AttachmentUrlParameters param, CloudStorageContainerType cloudStorageContainerType) =>
                {
                    externalDocumentId = documentId;
                })
                .Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            AzureUploadUrlContainer azureUploadUrlContainer = documentStorageProvider.GetUploadSasUri<Activity>(parameters, EnumType.ActivityDocumentType, CloudStorageContainerType.Activity);

            // Assert
            Assert.Equal(externalDocumentId, azureUploadUrlContainer.ExternalDocumentId);
        }
    }
}