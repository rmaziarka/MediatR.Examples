namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    using KnightFrank.Foundation.Antares.Cloud.Storage;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    public class StorageClientWrapper : IStorageClientWrapper
    {
        private StorageClient storageClient;

        public StorageClientWrapper()
        {
            this.storageClient = new StorageClient();
        }

        public Uri GetSasUri(ICloudBlobResource blobResource, SharedAccessBlobPolicy sharedAccessBlobPolicy)
        {
            string uri = this.storageClient.BlobClient.GetBlobSasUri(blobResource, sharedAccessBlobPolicy);

            return new Uri(uri);
        }
    }
}