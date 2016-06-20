namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using KnightFrank.Antares.Api.Models;

    public interface IStorageProvider
    {
        AzureUploadUrlContainer GetActivityUploadSasUri(AttachmentUrlParameters parameters);

        AzureDownloadUrlContainer GetActivityDownloadSasUri(AttachmentDownloadUrlParameters parameters);
    }
}