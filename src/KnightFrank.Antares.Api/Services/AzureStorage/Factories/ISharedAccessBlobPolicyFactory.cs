namespace KnightFrank.Antares.Api.Services.AzureStorage.Factories
{
    using Microsoft.WindowsAzure.Storage.Blob;

    public interface ISharedAccessBlobPolicyFactory
    {
        SharedAccessBlobPolicy Create();
    }
}