﻿namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Web.Http;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Domain.Enum.Types;

    [RoutePrefix("api/services")]
    public class ServicesController : ApiController
    {
        private readonly IStorageProvider storageProvider;

        public ServicesController(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        /// <summary>
        ///    Get destination url for uploading activity document
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("attachment/upload/activity/")]
        public Uri GetUrlForUploadFile(ActivityDocumentType activityDocumentType, [FromUri] AttachmentUrlParameters parameters)
        {
            // TODO: When filename is required?
            return this.storageProvider.GetActivitySasUri(activityDocumentType, parameters);
        }
    }
}