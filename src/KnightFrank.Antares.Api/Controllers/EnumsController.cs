namespace KnightFrank.Antares.Api.Controllers
{
    using System.Web.Http;

    using KnightFrank.Antares.Domain.Enum;
    using KnightFrank.Antares.Domain.Enum.Queries;

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
        /// <param name="query">The query.</param>
        /// <returns>Enum Query Result.</returns>
        [HttpGet]
        [Route("{code}/items")]
        public EnumQueryResult GetEnumItemsByCode([FromUri] EnumQuery query)
        {
            return this.mediator.Send(query);
        }
    }
}