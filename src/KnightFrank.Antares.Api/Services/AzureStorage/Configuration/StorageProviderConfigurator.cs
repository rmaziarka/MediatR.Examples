namespace KnightFrank.Antares.Api.Services.AzureStorage.Configuration
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Api.Controllers;
    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public class StorageProviderConfigurator : IStorageProviderConfigurator
    {
        private IStorageProvider storageProvider;

        public delegate AzureUploadUrlContainer GetUploadSasUri(AttachmentUrlParameters parameters);
        public delegate AzureDownloadUrlContainer GetDownloadSasUri(AttachmentDownloadUrlParameters parameters);

        public StorageProviderConfigurator(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        public Dictionary<CloudStorageContainerType, GetUploadSasUri> ConfigureUploadUrlContainers()
        {
            var getUploadSasUriDictionary = new Dictionary<CloudStorageContainerType, GetUploadSasUri>();

            getUploadSasUriDictionary.Add(CloudStorageContainerType.Activity, (x) => this.storageProvider.GetUploadSasUri<Activity>(x, EnumType.ActivityDocumentType, CloudStorageContainerType.Activity));

            return getUploadSasUriDictionary;
        }

        public Dictionary<CloudStorageContainerType, GetDownloadSasUri> ConfigureDownloadUrlContainers()
        {
            var getDownloadSasUriDictionary = new Dictionary<CloudStorageContainerType, GetDownloadSasUri>();

            getDownloadSasUriDictionary.Add(CloudStorageContainerType.Activity, (x) => this.storageProvider.GetDownloadSasUri<Activity>(x, EnumType.ActivityDocumentType, CloudStorageContainerType.Activity));

            return getDownloadSasUriDictionary;
        }
    }
}