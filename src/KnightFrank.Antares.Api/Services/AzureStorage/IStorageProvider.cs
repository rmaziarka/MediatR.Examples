namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public interface IStorageProvider
    {
        AzureUploadUrlContainer GetUploadSasUri<T>(AttachmentUrlParameters parameters, EnumType entityDocumentType, CloudStorageContainerType cloudStorageContainerType) where T : BaseEntity;

        AzureDownloadUrlContainer GetDownloadSasUri<T>(AttachmentDownloadUrlParameters parameters, EnumType entityDocumentType, CloudStorageContainerType cloudStorageContainerType) where T : BaseEntity;
    }
}