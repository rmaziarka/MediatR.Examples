﻿namespace KnightFrank.Antares.Api.Controllers
{
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
    }
}