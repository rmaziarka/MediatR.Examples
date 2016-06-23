namespace KnightFrank.Antares.Api.Services.AzureStorage.Factories
{
    using System;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Objects;

    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public class BlobResourceFactory : IBlobResourceFactory
    {
        public ICloudBlobResource Create(DocumentType documentType, Guid externalDocumentId, AttachmentUrlParameters parameters, CloudStorageContainerType cloudStorageContainerType)
        {
            ICloudBlobResource blobResource;

            switch (documentType)
            {
                case DocumentType.Brochure:
                    {
                        blobResource = new CloudBlobBrochureDocumentResource(
                            cloudStorageContainerType,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            externalDocumentId.ToString(),
                            parameters.Filename);

                        break;
                    }
                case DocumentType.Epc:
                    {
                        blobResource = new CloudBlobEPCGraphDocumentResource(
                            cloudStorageContainerType,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            externalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case DocumentType.FloorPlan:
                    {
                        blobResource = new CloudBlobFloorPlanDocumentResource(
                            cloudStorageContainerType,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            externalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case DocumentType.Photograph:
                    {
                        blobResource = new CloudBlobImageResource(
                            cloudStorageContainerType,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            externalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case DocumentType.VideoTour:
                    {
                        blobResource = new CloudBlobVideoResource(
                            cloudStorageContainerType,
                            VideoFormat.Mp4, // not used
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            externalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case DocumentType.MarketingSignOff:
                case DocumentType.CddDocument:
                case DocumentType.GasCertificate:
                case DocumentType.TermsOfBusiness:
                    {
                        blobResource = new CloudBlobGeneralDocumentResource(
                            cloudStorageContainerType,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            externalDocumentId.ToString(),
                            parameters.Filename);

                        break;
                    }
                default:
                    {
                        throw new DomainValidationException("documentType", "Document type is not supported");
                    }
            }

            return blobResource;
        }
    }
}