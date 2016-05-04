namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage.Factories
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Objects;

    using Xunit;


    public class BlobResourceFactoryTests
    {
        public static readonly IEnumerable<object[]> ActivityTestData = new List<object[]>
         {
            new object[] {ActivityDocumentType.Brochure, typeof(CloudBlobBrochureDocumentResource) },
            new object[] {ActivityDocumentType.MarketingSignOff, typeof(CloudBlobGeneralDocumentResource) },
            new object[] {ActivityDocumentType.CddDocument, typeof(CloudBlobGeneralDocumentResource) },
            new object[] {ActivityDocumentType.GasCertificate, typeof(CloudBlobGeneralDocumentResource) },
            new object[] {ActivityDocumentType.Photograph, typeof(CloudBlobImageResource) },
            new object[] {ActivityDocumentType.FloorPlan, typeof(CloudBlobFloorPlanDocumentResource) },
            new object[] {ActivityDocumentType.Epc, typeof(CloudBlobEPCGraphDocumentResource) },
            new object[] {ActivityDocumentType.VideoTour, typeof(CloudBlobVideoResource) },
            new object[] {ActivityDocumentType.TermsOfBusiness, typeof(CloudBlobGeneralDocumentResource) },
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