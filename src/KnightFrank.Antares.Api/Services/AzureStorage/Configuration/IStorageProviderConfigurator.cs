namespace KnightFrank.Antares.Api.Services.AzureStorage.Configuration
{
    using System.Collections.Generic;

    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public interface IStorageProviderConfigurator
    {
        Dictionary<CloudStorageContainerType, StorageProviderConfigurator.GetUploadSasUri> ConfigureUploadUrlContainers();

        Dictionary<CloudStorageContainerType, StorageProviderConfigurator.GetDownloadSasUri> ConfigureDownloadUrlContainers();
    }
}