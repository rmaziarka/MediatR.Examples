namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Web.Http;

    using MediatR;
    using Domain.Requirement.Commands;

    /// <summary>
    /// Requirement controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RequirementController : ApiController
    {
        private IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequirementController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public RequirementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create requirement
        /// </summary>
        /// <returns>Requirement identifier.</returns>
        [HttpPost]
        public Guid CreateRequirement(CreateRequirementCommand command)
        {
            return this.mediator.Send(command);
        }
    }
}
