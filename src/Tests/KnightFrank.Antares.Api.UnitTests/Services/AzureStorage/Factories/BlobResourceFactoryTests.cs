namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage.Factories
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Objects;

    using Xunit;

    public class BlobResourceFactoryTests
    {
        public static readonly IEnumerable<object[]> ActivityTestData = new List<object[]>
         {
            new object[] {DocumentType.Brochure, typeof(CloudBlobBrochureDocumentResource) },
            new object[] {DocumentType.MarketingSignOff, typeof(CloudBlobGeneralDocumentResource) },
            new object[] {DocumentType.CddDocument, typeof(CloudBlobGeneralDocumentResource) },
            new object[] {DocumentType.GasCertificate, typeof(CloudBlobGeneralDocumentResource) },
            new object[] {DocumentType.Photograph, typeof(CloudBlobImageResource) },
            new object[] {DocumentType.FloorPlan, typeof(CloudBlobFloorPlanDocumentResource) },
            new object[] {DocumentType.Epc, typeof(CloudBlobEPCGraphDocumentResource) },
            new object[] {DocumentType.VideoTour, typeof(CloudBlobVideoResource) },
            new object[] {DocumentType.TermsOfBusiness, typeof(CloudBlobGeneralDocumentResource) },
         };

        [Theory]
        [MemberData("ActivityTestData")]
        public void Given_ActivityDocumentType_ThenCorrectTypeShouldBeCreated(DocumentType documentType, Type type)
        {
            Guid externalDocumentId = Guid.NewGuid();
            var parameters = new AttachmentUrlParameters { Filename = "filename" };

            var blobResourceFactory = new BlobResourceFactory();
            ICloudBlobResource cloudBlobResource = blobResourceFactory.Create(documentType, externalDocumentId, parameters, CloudStorageContainerType.Activity);

            Assert.Equal(type, cloudBlobResource.GetType());
        }
    }
}