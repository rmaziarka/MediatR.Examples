namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using MediatR;

    /// <summary>
    ///     Enums controller.
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
        ///     Gets ditionary of enums
        /// </summary>
        /// <returns>Dictionary of enums.</returns>
        [HttpGet]
        [Route("")]
        public Dictionary<string, ICollection<EnumItemResult>> GetEnumItems()
        {
            var query = new EnumItemQuery();
            return this.mediator.Send(query);
        }
    }
}
