namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.Enum.Queries;

    using MediatR;

    /// <summary>
    ///     Translations controller.
    /// </summary>
    [RoutePrefix("api/translations")]
    public class TranslationsController : ApiController
    {
        private readonly IMediator mediator;

        public TranslationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Gets translations for enums by isoCode.
        /// </summary>
        /// <param name="isoCode">IsoCode</param>
        /// <returns>Dictionary of translations.</returns>
        [HttpGet]
        [Route("enums/{isoCode?}")]
        public Dictionary<Guid, string> GetEnums(string isoCode = null)
        {
            var query = new EnumLocalisedQuery { IsoCode = isoCode };
            return this.mediator.Send(query);
        }
    }
}
