namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Web.Http;

    using FluentValidation;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Validators.Services;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    [RoutePrefix("api/services")]
    public class ServicesController : ApiController
    {
        private readonly IDocumentStorageProvider documentStorageProvider;

        public ServicesController(IDocumentStorageProvider documentStorageProvider)
        {
            this.documentStorageProvider = documentStorageProvider;

            this.documentStorageProvider.ConfigureUploadUrl();
            this.documentStorageProvider.ConfigureDownloadUrl();
        }

        /// <summary>
        ///    Get destination url for uploading document
        /// </summary>
        /// <param name="entity">The name of the entity the attachment is related to.</param>
        /// <param name="parameters">The parameters query.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("attachment/upload/{entity}/")]
        public AzureUploadUrlContainer GetUrlForUploadFile(CloudStorageContainerType entity, [FromUri(Name = "")] AttachmentUrlParameters parameters)
        {
            var validator = new AttachmentUrlParametersValidator();
            validator.ValidateAndThrow(parameters);

            parameters.cloudStorageContainerType = entity;

            return this.documentStorageProvider.GetUploadUrlMethod(parameters.cloudStorageContainerType)(parameters);
        }

        /// <summary>
        ///    Get destination url for downloading document
        /// </summary>
        /// <param name="entity">The name of the entity the attachment is related to.</param>
        /// <param name="parameters">The parameters query.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("attachment/download/{entity}/")]
        public AzureDownloadUrlContainer GetUrlForDownloadFile(CloudStorageContainerType entity, [FromUri(Name = "")] AttachmentDownloadUrlParameters parameters)
        {
            var validator = new AttachmentDownloadUrlParametersValidator();
            validator.ValidateAndThrow(parameters);

            parameters.cloudStorageContainerType = entity;

            return this.documentStorageProvider.GetDownloadUrlMethod(parameters.cloudStorageContainerType)(parameters);
        }
    }
}