namespace KnightFrank.Antares.Api.Services.AzureStorage.Factories
{
    using System.Drawing.Imaging;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Enum.Types;
    using KnightFrank.Foundation.Antares;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Interfaces;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob.Objects;

    using Antares = KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public class BlobResourceFactory : IBlobResourceFactory
    {
        public ICloudBlobResource Create(ActivityDocumentType activityDocumentType, AttachmentUrlParameters parameters)
        {
            ICloudBlobResource blobResource;

            switch (activityDocumentType)
            {
                case ActivityDocumentType.Brochure:
                    {
                        blobResource = new CloudBlobBrochureDocumentResource(
                            Antares.CloudStorageContainerType.Activity,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            parameters.ExternalDocumentId.ToString(),
                            parameters.Filename);

                        break;
                    }
                case ActivityDocumentType.Epc:
                    {
                        blobResource = new CloudBlobEPCGraphDocumentResource(
                           Antares.CloudStorageContainerType.Activity,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            parameters.ExternalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case ActivityDocumentType.FloorPlan:
                    {
                        blobResource = new CloudBlobFloorPlanDocumentResource(
                           Antares.CloudStorageContainerType.Activity,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            parameters.ExternalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case ActivityDocumentType.Photograph:
                    {
                        blobResource = new CloudBlobImageResource(
                            Antares.CloudStorageContainerType.Activity,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            parameters.ExternalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case ActivityDocumentType.VideoTour:
                    {
                        blobResource = new CloudBlobVideoResource(
                            Antares.CloudStorageContainerType.Activity,
                            VideoFormat.Mp4, // not used
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            parameters.ExternalDocumentId.ToString(),
                            parameters.Filename);
                        break;
                    }
                case ActivityDocumentType.MarketingSignOff:
                case ActivityDocumentType.CddDocument:
                case ActivityDocumentType.GasCertificate:
                case ActivityDocumentType.TermsOfBusiness:
                    {
                        blobResource = new CloudBlobGeneralDocumentResource(
                            Antares.CloudStorageContainerType.Activity,
                            parameters.LocaleIsoCode,
                            parameters.EntityReferenceId.ToString(),
                            parameters.ExternalDocumentId.ToString(),
                            parameters.Filename);

                        break;
                    }
                default:
                    {
                        throw new DomainValidationException("activityDocumentType", "Activity type is not supported");
                    }
            }

            return blobResource;

        }
    }
}