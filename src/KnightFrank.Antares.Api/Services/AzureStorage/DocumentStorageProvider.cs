namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    using Microsoft.WindowsAzure.Storage.Blob;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class DocumentStorageProvider : IStorageProvider
    {
        private readonly IStorageClientWrapper storageClient;
        private readonly IBlobResourceFactory blobResourceFactory;
        private readonly ISharedAccessBlobPolicyFactory sharedAccessBlobPolicyFactory;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public DocumentStorageProvider(
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

        public AzureUploadUrlContainer GetUploadSasUri<T>(AttachmentUrlParameters parameters, EnumType entityDocumentType, CloudStorageContainerType cloudStorageContainerType) where T : BaseEntity
        {
            this.entityValidator.EntityExists<T>(parameters.EntityReferenceId);

            DocumentType documentType = this.GetDocumentType(parameters, entityDocumentType);

            Guid externalDocumentId = Guid.NewGuid();

            return new AzureUploadUrlContainer
            {
                Url = this.GetSasUrl(parameters, externalDocumentId, documentType, cloudStorageContainerType),
                ExternalDocumentId = externalDocumentId
            };
        }

        public AzureDownloadUrlContainer GetDownloadSasUri<T>(AttachmentDownloadUrlParameters parameters, EnumType entityDocumentType, CloudStorageContainerType cloudStorageContainerType) where T : BaseEntity
        {
            this.entityValidator.EntityExists<T>(parameters.EntityReferenceId);

            DocumentType documentType = this.GetDocumentType(parameters, entityDocumentType);

            return new AzureDownloadUrlContainer
            {
                Url = this.GetSasUrl(parameters, parameters.ExternalDocumentId, documentType, cloudStorageContainerType),
            };
        }

        private DocumentType GetDocumentType(AttachmentUrlParameters parameters, EnumType entityDocumentType)
        {
            EnumTypeItem enumTypeItem =
                this.enumTypeItemRepository
                .GetWithInclude(x => x.Id == parameters.DocumentTypeId && x.EnumType.Code == entityDocumentType.ToString(), x => x.EnumType)
                .SingleOrDefault();

            this.enumTypeItemValidator.ItemExists(enumTypeItem, parameters.DocumentTypeId);

            var documentType = (DocumentType)Enum.Parse(typeof(DocumentType), enumTypeItem.Code, true);

            return documentType;
        }

        private Uri GetSasUrl(AttachmentUrlParameters urlParameters, Guid externalDocumentId, DocumentType documentType, CloudStorageContainerType cloudStorageContainerType)
        {
            ICloudBlobResource cloudBlobResource = this.blobResourceFactory.Create(documentType, externalDocumentId, urlParameters, cloudStorageContainerType);
            SharedAccessBlobPolicy sharedAccessBlobPolicy = this.sharedAccessBlobPolicyFactory.Create();

            return this.storageClient.GetSasUri(cloudBlobResource, sharedAccessBlobPolicy);
        }
    }
}