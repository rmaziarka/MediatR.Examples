namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage
{
    using System;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class StorageProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_GetActivitySasUri_Then_ShouldDelegateBehaviour(
            [Frozen] Mock<IStorageClientWrapper> storageClient,
            [Frozen] Mock<IBlobResourceFactory> blobResourceFactory,
            [Frozen] Mock<ISharedAccessBlobPolicyFactory> sharedAccessBlobPolicyFactory,
            ICloudBlobResource resource,
            AttachmentUrlParameters parameters,
            SharedAccessBlobPolicy policy,
            StorageProvider storageProvider
            )   
        {
            // Arrange
            var activityDocumentType = ActivityDocumentType.Brochure;

            blobResourceFactory.Setup(x => x.Create(activityDocumentType, parameters)).Returns(resource);
            sharedAccessBlobPolicyFactory.Setup(x => x.Create()).Returns(policy);

            storageClient.Setup(x =>
                x.GetSasUri(It.IsAny<ICloudBlobResource>(), It.IsAny<SharedAccessBlobPolicy>())).Returns(new Uri("http://www.google.com"));

            // Act
            storageProvider.GetActivitySasUri(activityDocumentType, parameters);

            // Assert
            blobResourceFactory.Verify(x => x.Create(activityDocumentType, parameters));
            storageClient.Verify(x => x.GetSasUri(resource, policy));

        }
    }
}