namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    using KnightFrank.Foundation.Cloud.Storage.Blob.Objects.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    public interface IStorageClientWrapper
    {
        Uri GetSasUri(ICloudBlobResource blobResource, SharedAccessBlobPolicy sharedAccessBlobPolicy);
    }
}