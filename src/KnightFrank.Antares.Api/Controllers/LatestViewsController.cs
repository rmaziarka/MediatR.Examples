namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.LatestView.Commands;
    using KnightFrank.Antares.Domain.LatestView.Queries;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    using MediatR;

    /// <summary>
    /// Latest entity views controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/latestviews")]
    public class LatestViewsController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatestViewsController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public LatestViewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates the latest view.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IEnumerable<LatestViewQueryResultItem> CreateLatestView(CreateLatestViewCommand command)
        {
            this.mediator.Send(command);
            return this.GetLatest();
        }

        /// <summary>
        /// Gets latest views for all entities.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<LatestViewQueryResultItem> GetLatest()
        {
            return this.mediator.Send(new LatestViewsQuery());
        }
    }
}
