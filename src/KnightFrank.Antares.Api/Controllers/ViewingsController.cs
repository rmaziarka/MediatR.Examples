namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using MediatR;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Viewing.Queries;
    using Domain.Viewing.Commands;
    /// <summary>
    /// Viewings controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/viewings")]
    public class ViewingsController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewingsController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public ViewingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create viewing
        /// </summary>
        /// <returns>Viewing.</returns>
        [HttpPost]
        [Route("")]
        public Viewing CreateViewing(CreateViewingCommand command)
        {
            Guid viewingId =  this.mediator.Send(command);

            return this.GetViewingById(viewingId);
        }

        /// <summary>
        /// Gets the viewing by Id.
        /// </summary>
        /// <param name="id">Viewing Id.</param>
        /// <returns>Viewing.</returns>
        [HttpGet]
        [Route("{id}")]
        public Viewing GetViewingById(Guid id)
        {
            Viewing viewing = this.mediator.Send(new ViewingQuery { Id = id });

            if (viewing == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Viewing not found."));
            }

            return viewing;
        }
    }
}
