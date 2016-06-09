namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using FluentValidation;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Services.AzureStorage.Configuration;
    using KnightFrank.Antares.Api.Validators.Services;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    [RoutePrefix("api/services")]
    public class ServicesController : ApiController
    {
        private readonly Dictionary<CloudStorageContainerType, StorageProviderConfigurator.GetUploadSasUri> uploadUrlContainersDictionary;
        private readonly Dictionary<CloudStorageContainerType, StorageProviderConfigurator.GetDownloadSasUri> downloadUrlContainersDictionary;

        public ServicesController(IStorageProviderConfigurator storageProviderConfigurator)
        {
            this.uploadUrlContainersDictionary = storageProviderConfigurator.ConfigureUploadUrlContainers();
            this.downloadUrlContainersDictionary = storageProviderConfigurator.ConfigureDownloadUrlContainers();
        }

        /// <summary>
        ///    Get destination url for uploading activity document
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("attachment/upload/activity/")]
        public AzureUploadUrlContainer GetUrlForUploadFile([FromUri(Name = "")] AttachmentUrlParameters parameters)
        {
            var validator = new AttachmentUrlParametersValidator();
            validator.ValidateAndThrow(parameters);

            //TODO: get entity name form uri
            CloudStorageContainerType cloudStorageContainerType;
            Enum.TryParse("Activity", out cloudStorageContainerType);

            return this.uploadUrlContainersDictionary[cloudStorageContainerType](parameters);
        }

        /// <summary>
        ///    Get destination url for downloading activity document
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("attachment/download/activity/")]
        public AzureDownloadUrlContainer GetUrlForDownloadFile([FromUri(Name = "")] AttachmentDownloadUrlParameters parameters)
        {
            // TODO: DocumentTypeId, EntityReferenceId and Filename should be fetched from DB, not sent from client.

            var validator = new AttachmentDownloadUrlParametersValidator();
            validator.ValidateAndThrow(parameters);

            //TODO: get entity name form uri
            CloudStorageContainerType cloudStorageContainerType;
            Enum.TryParse("Activity", out cloudStorageContainerType);

            return this.downloadUrlContainersDictionary[cloudStorageContainerType](parameters);
        }
    }
}