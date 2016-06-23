namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Domain.Common.Enums;

    public interface IEntityDocumentStorageProvider
    {
        AzureUploadUrlContainer GetUploadSasUri<T>(AttachmentUrlParameters parameters, EnumType entityDocumentType) where T : BaseEntity;

        AzureDownloadUrlContainer GetDownloadSasUri<T>(AttachmentDownloadUrlParameters parameters, EnumType entityDocumentType) where T : BaseEntity;
    }
}