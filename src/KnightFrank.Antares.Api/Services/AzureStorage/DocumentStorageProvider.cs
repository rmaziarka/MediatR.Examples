namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public class DocumentStorageProvider : IDocumentStorageProvider
    {
        private IEntityDocumentStorageProvider entityDocumentStorageProvider;

        public delegate AzureUploadUrlContainer GetUploadSasUri(AttachmentUrlParameters parameters);
        public delegate AzureDownloadUrlContainer GetDownloadSasUri(AttachmentDownloadUrlParameters parameters);

        private readonly Dictionary<CloudStorageContainerType, GetUploadSasUri> uploadUrlDictionary;
        private readonly Dictionary<CloudStorageContainerType, GetDownloadSasUri> downloadUrlDictionary;

        public DocumentStorageProvider(IEntityDocumentStorageProvider entityDocumentStorageProvider)
        {
            this.entityDocumentStorageProvider = entityDocumentStorageProvider;

            this.uploadUrlDictionary = new Dictionary<CloudStorageContainerType, GetUploadSasUri>();
            this.downloadUrlDictionary = new Dictionary<CloudStorageContainerType, GetDownloadSasUri>();
        }

        public void ConfigureUploadUrl()
        {
            this.uploadUrlDictionary.Add(CloudStorageContainerType.Activity, (x) => this.entityDocumentStorageProvider.GetUploadSasUri<Activity>(x, EnumType.ActivityDocumentType));
            this.uploadUrlDictionary.Add(CloudStorageContainerType.Property, (x) => this.entityDocumentStorageProvider.GetUploadSasUri<Property>(x, EnumType.PropertyDocumentType));
        }

        public void ConfigureDownloadUrl()
        {
            this.downloadUrlDictionary.Add(CloudStorageContainerType.Activity, (x) => this.entityDocumentStorageProvider.GetDownloadSasUri<Activity>(x, EnumType.ActivityDocumentType));
            this.downloadUrlDictionary.Add(CloudStorageContainerType.Property, (x) => this.entityDocumentStorageProvider.GetDownloadSasUri<Property>(x, EnumType.PropertyDocumentType));
        }

        public GetUploadSasUri GetUploadUrlMethod(CloudStorageContainerType cloudStorageContainerType)
        {
            GetUploadSasUri method;
            if (!this.uploadUrlDictionary.TryGetValue(cloudStorageContainerType, out method))
            {
                throw new DomainValidationException("entity", "Entity is not supported");
            };

            return method;
        }

        public GetDownloadSasUri GetDownloadUrlMethod(CloudStorageContainerType cloudStorageContainerType)
        {
            GetDownloadSasUri method;
            if (!this.downloadUrlDictionary.TryGetValue(cloudStorageContainerType, out method))
            {
                throw new DomainValidationException("entity", "Entity is not supported");
            };

            return method;
        }
    }
}