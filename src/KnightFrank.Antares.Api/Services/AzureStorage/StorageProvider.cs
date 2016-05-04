namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    public class StorageProvider : IStorageProvider
    {
        private readonly IStorageClientWrapper storageClient;
        private readonly IBlobResourceFactory blobResourceFactory;
        private readonly ISharedAccessBlobPolicyFactory sharedAccessBlobPolicyFactory;

        public StorageProvider(IStorageClientWrapper storageClient, IBlobResourceFactory blobResourceFactory,
            ISharedAccessBlobPolicyFactory sharedAccessBlobPolicyFactory)
        {
            this.storageClient = storageClient;
            this.blobResourceFactory = blobResourceFactory;
            this.sharedAccessBlobPolicyFactory = sharedAccessBlobPolicyFactory;
        }

        public Uri GetActivitySasUri(ActivityDocumentType activityDocumentType, AttachmentUrlParameters parameters)
        {
            ICloudBlobResource cloudBlobResource = this.blobResourceFactory.Create(activityDocumentType, parameters);
            SharedAccessBlobPolicy sharedAccessBlobPolicy = this.sharedAccessBlobPolicyFactory.Create();

            return this.storageClient.GetSasUri(cloudBlobResource, sharedAccessBlobPolicy);
        }
    }
}