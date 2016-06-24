namespace KnightFrank.Antares.Api.Services.AzureStorage.Factories
{
    using System;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public interface IBlobResourceFactory
    {
        ICloudBlobResource Create(DocumentType documentType, Guid externalDocumentId, AttachmentUrlParameters parameters, CloudStorageContainerType cloudStorageContainerType);
    }
}