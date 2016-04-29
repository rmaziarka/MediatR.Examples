namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Domain.Enum.Types;

    public interface IStorageProvider
    {
        Uri GetActivitySasUri(ActivityDocumentType activityDocumentType, AttachmentUrlParameters parameters);
    }
}