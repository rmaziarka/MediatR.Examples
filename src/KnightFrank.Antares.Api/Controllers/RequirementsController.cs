namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using MediatR;
    using Domain.Requirement.Commands;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Requirement.Queries;

    /// <summary>
    /// Requirement controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/requirements")]
    public class RequirementsController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequirementsController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public RequirementsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create requirement
        /// </summary>
        /// <returns>Requirement identifier.</returns>
        [HttpPost]
        public Requirement CreateRequirement(CreateRequirementCommand command)
        {
            Guid requirementId =  this.mediator.Send(command);

            return this.GetRequirementById(requirementId);
        }

        /// <summary>
        /// Gets the requirement by Id.
        /// </summary>
        /// <param name="id">Requirement Id.</param>
        /// <returns>Requirement.</returns>
        [HttpGet]
        [Route("{id}")]
        public Requirement GetRequirementById(Guid id)
        {
            Requirement requirement = this.mediator.Send(new RequirementQuery { Id = id });

            if (requirement == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Requirement not found."));
            }

            return requirement;
        }
    }
}
