namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    public interface IStorageClientWrapper
    {
        Uri GetSasUri(ICloudBlobResource blobResource, SharedAccessBlobPolicy sharedAccessBlobPolicy);
    }
}