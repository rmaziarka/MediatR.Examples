namespace KnightFrank.Antares.Api.Services.AzureStorage.Factories
{
    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Cloud.Storage.Blob.Objects.Interfaces;

    public interface IBlobResourceFactory
    {
        ICloudBlobResource Create(ActivityDocumentType activityDocumentType, AttachmentUrlParameters parameters);
    }
}