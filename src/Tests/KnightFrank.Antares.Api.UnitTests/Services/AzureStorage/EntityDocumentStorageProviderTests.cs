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
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class EntityDocumentStorageProviderTests
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
            EntityDocumentStorageProvider entityDocumentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            EnumTypeItem enumTypeItem = this.SetupEnumTypeItemForDocumentType(documentType, enumTypeItemRepository);

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);
            storageClient.Setup(x => x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            parameters.cloudStorageContainerType = CloudStorageContainerType.Activity;

            // Act
            entityDocumentStorageProvider.GetUploadSasUri<Activity>(parameters, EnumType.ActivityDocumentType);

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
            EntityDocumentStorageProvider entityDocumentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            EnumTypeItem enumTypeItem = this.SetupEnumTypeItemForDocumentType(documentType, enumTypeItemRepository);

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);
            storageClient.Setup(x => x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            parameters.cloudStorageContainerType = CloudStorageContainerType.Activity;

            // Act
            entityDocumentStorageProvider.GetDownloadSasUri<Activity>(parameters, EnumType.ActivityDocumentType);

            // Assert
            blobResourceFactory.Verify(x => x.Create(documentType, parameters.ExternalDocumentId, parameters, CloudStorageContainerType.Activity), Times.Once);
            storageClient.Verify(x => x.GetSasUri(resource, policy), Times.Once);
            enumTypeItemValidator.Verify(x => x.ItemExists(enumTypeItem, parameters.DocumentTypeId), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Activity>(parameters.EntityReferenceId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_GetDocumentUploadSasUri_Then_ShouldReturnProperData(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            DocumentType documentType,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            EntityDocumentStorageProvider entityDocumentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            this.SetupEnumTypeItemForDocumentType(documentType, enumTypeItemRepository);

            Guid externalDocumentId = Guid.Empty;

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity))
                .Callback((DocumentType docType, Guid documentId, AttachmentUrlParameters param, CloudStorageContainerType cloudStorageContainerType) =>
                {
                    externalDocumentId = documentId;
                })
                .Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x => x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            parameters.cloudStorageContainerType = CloudStorageContainerType.Activity;

            // Act
            AzureUploadUrlContainer azureUploadUrlContainer = entityDocumentStorageProvider.GetUploadSasUri<Activity>(parameters, EnumType.ActivityDocumentType);

            // Assert
            Assert.Equal(externalDocumentId, azureUploadUrlContainer.ExternalDocumentId);
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
            EntityDocumentStorageProvider entityDocumentStorageProvider,
            Uri uri
            )
        {
            // Arrange
            this.SetupEnumTypeItemForDocumentType(documentType, enumTypeItemRepository);

            blobResourceFactory.Setup(x => x.Create(documentType, It.IsAny<Guid>(), parameters, CloudStorageContainerType.Activity)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x => x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(uri);

            parameters.cloudStorageContainerType = CloudStorageContainerType.Activity;

            // Act
            AzureDownloadUrlContainer container = entityDocumentStorageProvider.GetDownloadSasUri<Activity>(parameters, EnumType.ActivityDocumentType);

            // Assert
            Assert.Equal(uri, container.Url);
        }

        private EnumTypeItem SetupEnumTypeItemForDocumentType(DocumentType documentType, Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository)
        {
            var enumTypeItem = new EnumTypeItem
            {
                Code = documentType.ToString()
            };

            var enumTypeItems = new List<EnumTypeItem> { enumTypeItem };

            enumTypeItemRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumTypeItem, bool>>>(), e => e.EnumType))
                .Returns(enumTypeItems);

            return enumTypeItem;
        }
    }
}