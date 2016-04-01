namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using MediatR;

    /// <summary>
    /// Enums controller.
    /// </summary>
    [RoutePrefix("api/enums")]
    public class EnumsController : ApiController
    {
        private readonly IMediator mediator;

        public EnumsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the enum items by code.
        /// </summary>
        /// <param name="code">Enum item code</param>
        /// <returns>Enum Query Result.</returns>
        [HttpGet]
        [Route("{code}/items")]
        public EnumQueryResult GetEnumItemsByCode(string code)
        {
            var query = new EnumQuery { Code = code };
            return this.mediator.Send(query);
        }

        /// <summary>
        ///     Gets translations for enum localised by isoCode.
        /// </summary>
        /// <param name="isoCode">IsoCode</param>
        /// <returns>Dictionary of translations.</returns>
        [HttpGet]
        [Route("translations/{isoCode?}")]
        public Dictionary<Guid, string> GetLocalizedEnums(string isoCode = null)
        {
            var query = new EnumLocalisedQuery { IsoCode = isoCode };
            return this.mediator.Send(query);
        }
    }
}
