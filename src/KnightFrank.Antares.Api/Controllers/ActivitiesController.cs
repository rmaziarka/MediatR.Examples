namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using MediatR;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    public class ActivitiesController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Contacts controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        public ActivitiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="command">Contact entity</param>
        [HttpPost]
        public Activity CreateActivity([FromBody] CreateActivityCommand command)
        {
            Guid activityId = this.mediator.Send(command);
            return new Activity { Id = activityId };
        }
    }
}
