namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class StorageProvider : IStorageProvider
    {
        private readonly IStorageClientWrapper storageClient;
        private readonly IBlobResourceFactory blobResourceFactory;
        private readonly ISharedAccessBlobPolicyFactory sharedAccessBlobPolicyFactory;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public StorageProvider(
            IStorageClientWrapper storageClient, 
            IBlobResourceFactory blobResourceFactory,
            ISharedAccessBlobPolicyFactory sharedAccessBlobPolicyFactory,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.storageClient = storageClient;
            this.blobResourceFactory = blobResourceFactory;
            this.sharedAccessBlobPolicyFactory = sharedAccessBlobPolicyFactory;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
        }

        public AzureUploadUrlContainer GetActivityUploadSasUri(AttachmentUrlParameters parameters)
        {
            this.enumTypeItemValidator.ItemExists(EnumType.ActivityDocumentType, parameters.DocumentTypeId);

            ActivityDocumentType activityDocumentType = this.GetActivityDocumentType(parameters.DocumentTypeId);

            Guid externalDocumentId = Guid.NewGuid();
            ICloudBlobResource cloudBlobResource = this.blobResourceFactory.Create(activityDocumentType, externalDocumentId, parameters);
            SharedAccessBlobPolicy sharedAccessBlobPolicy = this.sharedAccessBlobPolicyFactory.Create();

            return new AzureUploadUrlContainer
            {
                Url = this.storageClient.GetSasUri(cloudBlobResource, sharedAccessBlobPolicy),
                ExternalDocumentId = externalDocumentId
            };
        }

        public AzureDownloadUrlContainer GetActivityDownloadSasUri(AttachmentDownloadUrlParameters parameters)
        {
            this.enumTypeItemValidator.ItemExists(EnumType.ActivityDocumentType, parameters.DocumentTypeId);

            ActivityDocumentType activityDocumentType = this.GetActivityDocumentType(parameters.DocumentTypeId);

            ICloudBlobResource cloudBlobResource = this.blobResourceFactory.Create(activityDocumentType, parameters.ExternalDocumentId, parameters);
            SharedAccessBlobPolicy sharedAccessBlobPolicy = this.sharedAccessBlobPolicyFactory.Create();

            return new AzureDownloadUrlContainer
            {
                Url = this.storageClient.GetSasUri(cloudBlobResource, sharedAccessBlobPolicy),
            };
        }

        private ActivityDocumentType GetActivityDocumentType(Guid documentTypeId)
        {
            EnumTypeItem enumTypeItem = this.enumTypeItemRepository.GetById(documentTypeId);

            ActivityDocumentType activityDocumentType;
            Enum.TryParse(enumTypeItem.Code, true, out activityDocumentType);
            return activityDocumentType;
        }
    }
}