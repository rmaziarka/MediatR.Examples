namespace KnightFrank.Antares.Api.IntegrationTests.MockExternalServices
{
    using System;

    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;

    using Microsoft.WindowsAzure.Storage.Blob;

    public class MockStorageClient : IStorageClientWrapper
    {
        public Uri GetSasUri(ICloudBlobResource blobResource, SharedAccessBlobPolicy sharedAccessBlobPolicy)
        {
            return new Uri("http://www.google.pl");
        }
    }
}
