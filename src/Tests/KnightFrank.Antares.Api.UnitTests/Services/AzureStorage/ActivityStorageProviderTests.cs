namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class ActivityStorageProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_GetActivityUploadSasUri_Then_ShouldDelegateBehaviour(
            [Frozen] Mock<IEnumTypeItemValidator> validator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            ActivityDocumentType activityDocumentType,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            ActivityStorageProvider activityStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = activityDocumentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(activityDocumentType, It.IsAny<Guid>(), parameters)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            activityStorageProvider.GetActivityUploadSasUri(parameters);

            // Assert
            blobResourceFactory.Verify(x => x.Create(activityDocumentType, It.IsAny<Guid>(), parameters), Times.Once);
            storageClient.Verify(x => x.GetSasUri(resource, policy), Times.Once);
            validator.Verify(x => x.ItemExists(enumTypeItem, parameters.DocumentTypeId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetActivityDownloadSasUri_Then_ShouldDelegateBehaviour(
            [Frozen] Mock<IEnumTypeItemValidator> validator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            ActivityDocumentType activityDocumentType,
            ICloudBlobResource resource,
            AttachmentDownloadUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            ActivityStorageProvider activityStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = activityDocumentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(activityDocumentType, It.IsAny<Guid>(), parameters)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            activityStorageProvider.GetActivityDownloadSasUri(parameters);

            // Assert
            blobResourceFactory.Verify(x => x.Create(activityDocumentType, parameters.ExternalDocumentId, parameters), Times.Once);
            storageClient.Verify(x => x.GetSasUri(resource, policy), Times.Once);
            validator.Verify(x => x.ItemExists(enumTypeItem, parameters.DocumentTypeId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetActivityUploadSasUri_Then_ShouldReturnCreatedUrl(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            ActivityDocumentType activityDocumentType,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            ActivityStorageProvider activityStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = activityDocumentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(activityDocumentType, It.IsAny<Guid>(), parameters)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            AzureUploadUrlContainer azureUploadUrlContainer = activityStorageProvider.GetActivityUploadSasUri(parameters);

            // Assert
            Assert.Equal(uri, azureUploadUrlContainer.Url);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetActivityDownloadSasUri_Then_ShouldReturnCreatedUrl(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            ActivityDocumentType activityDocumentType,
            ICloudBlobResource resource,
            AttachmentDownloadUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            ActivityStorageProvider activityStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = activityDocumentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            blobResourceFactory.Setup(x => x.Create(activityDocumentType, It.IsAny<Guid>(), parameters)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            AzureDownloadUrlContainer container = activityStorageProvider.GetActivityDownloadSasUri(parameters);

            // Assert
            Assert.Equal(uri, container.Url);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetActivityUploadSasUri_Then_ShouldReturnCreatedExternalId(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            ActivityDocumentType activityDocumentType,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            ActivityStorageProvider activityStorageProvider,
            Uri uri
            )
        {
            // Arrange
            var enumTypeItem = new EnumTypeItem
            {
                Code = activityDocumentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            Guid externalDocumentId = Guid.Empty;

            blobResourceFactory.Setup(x => x.Create(activityDocumentType, It.IsAny<Guid>(), parameters))
                .Callback((ActivityDocumentType documentType, Guid documentId, AttachmentUrlParameters param) =>
                {
                    externalDocumentId = documentId;
                })
                .Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            // Act
            AzureUploadUrlContainer azureUploadUrlContainer = activityStorageProvider.GetActivityUploadSasUri(parameters);

            // Assert
            Assert.Equal(externalDocumentId, azureUploadUrlContainer.ExternalDocumentId);
        }

    }
}