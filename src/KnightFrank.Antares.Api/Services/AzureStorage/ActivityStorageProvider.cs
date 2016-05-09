namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class ActivityStorageProvider : IStorageProvider
    {
        private readonly IStorageClientWrapper storageClient;
        private readonly IBlobResourceFactory blobResourceFactory;
        private readonly ISharedAccessBlobPolicyFactory sharedAccessBlobPolicyFactory;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public ActivityStorageProvider(
            IStorageClientWrapper storageClient, 
            IBlobResourceFactory blobResourceFactory,
            ISharedAccessBlobPolicyFactory sharedAccessBlobPolicyFactory,
            IEnumTypeItemValidator enumTypeItemValidator,
            IEntityValidator entityValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.storageClient = storageClient;
            this.blobResourceFactory = blobResourceFactory;
            this.sharedAccessBlobPolicyFactory = sharedAccessBlobPolicyFactory;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.entityValidator = entityValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
        }

        public AzureUploadUrlContainer GetActivityUploadSasUri(AttachmentUrlParameters parameters)
        {
            this.entityValidator.EntityExists<Activity>(parameters.EntityReferenceId);

            ActivityDocumentType activityDocumentType = this.GetActivityDocumentType(parameters.DocumentTypeId);

            Guid externalDocumentId = Guid.NewGuid();

            return new AzureUploadUrlContainer
            {
                Url = this.GetSasUrl(parameters, externalDocumentId, activityDocumentType),
                ExternalDocumentId = externalDocumentId
            };
        }

        public AzureDownloadUrlContainer GetActivityDownloadSasUri(AttachmentDownloadUrlParameters parameters)
        {
            this.entityValidator.EntityExists<Activity>(parameters.EntityReferenceId);

            ActivityDocumentType activityDocumentType = this.GetActivityDocumentType(parameters.DocumentTypeId);

            return new AzureDownloadUrlContainer
            {
                Url = this.GetSasUrl(parameters, parameters.ExternalDocumentId, activityDocumentType),
            };
        }

        private ActivityDocumentType GetActivityDocumentType(Guid documentTypeId)
        {
            EnumTypeItem enumTypeItem = 
                this.enumTypeItemRepository
                .GetWithInclude(x => x.Id == documentTypeId && x.EnumType.Code == EnumType.ActivityDocumentType.ToString(), x => x.EnumType)
                .SingleOrDefault();

            this.enumTypeItemValidator.ItemExists(enumTypeItem, documentTypeId);

            var activityDocumentType = (ActivityDocumentType)Enum.Parse(typeof(ActivityDocumentType), enumTypeItem.Code, true);

            return activityDocumentType;
        }

        private Uri GetSasUrl(AttachmentUrlParameters urlParameters, Guid externalDocumentId, ActivityDocumentType activityDocumentType)
        {
            ICloudBlobResource cloudBlobResource = this.blobResourceFactory.Create(activityDocumentType, externalDocumentId, urlParameters);
            SharedAccessBlobPolicy sharedAccessBlobPolicy = this.sharedAccessBlobPolicyFactory.Create();

            return this.storageClient.GetSasUri(cloudBlobResource, sharedAccessBlobPolicy);
        }
    }
}