namespace KnightFrank.Antares.Api.Services.AzureStorage
{
    using System;

    public class AzureUploadUrlContainer
    {
        public Uri Url { get; set; }
        public Guid ExternalDocumentId { get; set; }
    }
}