namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public interface IDocumentStorageProvider
    {
        void ConfigureUploadUrl();

        void ConfigureDownloadUrl();

        DocumentStorageProvider.GetUploadSasUri GetUploadUrlMethod(CloudStorageContainerType cloudStorageContainerType);

        DocumentStorageProvider.GetDownloadSasUri GetDownloadUrlMethod(CloudStorageContainerType cloudStorageContainerType);
    }
}