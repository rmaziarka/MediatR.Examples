namespace KnightFrank.Antares.Api.Services.AzureStorage.Factories
{
    using System;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Domain.Enum.Types;
    using Antares = KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    public interface IBlobResourceFactory
    {
        Antares.ICloudBlobResource Create(ActivityDocumentType activityDocumentType, Guid externalDocumentId, AttachmentUrlParameters parameters);
    }
}