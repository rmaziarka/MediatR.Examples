namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage.Factories
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Cloud.Storage.Blob.Objects.Interfaces;

    using Xunit;

    using Antares = KnightFrank.Foundation.Cloud.Storage.Blob.Objects.Antares;

    public class BlobResourceFactoryTests
    {
        public static readonly IEnumerable<object[]> ActivityTestData = new List<object[]>
         {
            new object[] {ActivityDocumentType.Brochure, typeof(Antares.CloudBlobBrochureDocumentResource) },
            new object[] {ActivityDocumentType.MarketingSignOff, typeof(Antares.CloudBlobGeneralResource) },
            new object[] {ActivityDocumentType.CddDocument, typeof(Antares.CloudBlobGeneralResource) },
            new object[] {ActivityDocumentType.GasCertificate, typeof(Antares.CloudBlobGeneralResource) },
         };

        [Theory]
        [MemberData("ActivityTestData")]
        public void Given_ActivityDocumentType_ThenCorrectTypeShouldBeCreated(ActivityDocumentType documentType, Type type)
        {
            var parameters = new AttachmentUrlParameters { Filename = "filename" };

            var blobResourceFactory = new BlobResourceFactory();
            ICloudBlobResource cloudBlobResource = blobResourceFactory.Create(documentType, parameters);

            Assert.Equal(type, cloudBlobResource.GetType());
        }
    }
}