namespace KnightFrank.Antares.Api.Controllers
{
    using System.Web.Http;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;

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
        public AzureUploadUrlContainer GetUrlForUploadFile([FromUri(Name = "")] AttachmentUrlParameters parameters)
        {
            return this.storageProvider.GetActivityUploadSasUri(parameters);
        }

        /// <summary>
        ///    Get destination url for downloading activity document
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("attachment/download/activity/")]
        public AzureDownloadUrlContainer GetUrlForDownloadFile([FromUri(Name = "")] AttachmentDownloadUrlParameters parameters)
        {
            return this.storageProvider.GetActivityDownloadSasUri(parameters);
        }
    }
}