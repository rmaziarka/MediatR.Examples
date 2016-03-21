namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.Ownership.Commands;

    using MediatR;

    /// <summary>
    /// Ownership controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/ownership")]
    public class OwnershipController : ApiController
    {
        private readonly IMediator mediator;

        public OwnershipController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create requirement
        /// </summary>
        /// <returns>Requirement identifier.</returns>
        [HttpPost]
        public Guid CreateOwnership(CreateOwnershipCommand command)
        {
            return this.mediator.Send(command);
        }
    }
}
