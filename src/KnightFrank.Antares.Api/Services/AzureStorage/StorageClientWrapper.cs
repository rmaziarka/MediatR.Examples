namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    using KnightFrank.Foundation.Cloud.Storage.Blob.Objects.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    public class StorageClientWrapper : IStorageClientWrapper
    {
        private Foundation.Cloud.Storage.StorageClient storageClient;

        public StorageClientWrapper()
        {
            this.storageClient = new Foundation.Cloud.Storage.StorageClient();
        }

        public Uri GetSasUri(ICloudBlobResource blobResource, SharedAccessBlobPolicy sharedAccessBlobPolicy)
        {
            string uri = this.storageClient.BlobClient.GetBlobSasUri(blobResource, sharedAccessBlobPolicy);

            return new Uri(uri);
        }
    }
}