namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Activity.Queries;

    using MediatR;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    [RoutePrefix("api/activities")]
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
        ///     Create contact
        /// </summary>
        /// <param name="command">Contact entity</param>
        [HttpPost]
        [Route("")]
        public Activity CreateActivity([FromBody] CreateActivityCommand command)
        {
            Guid activityId = this.mediator.Send(command);

            return this.GetActivity(activityId);
        }

        /// <summary>
        ///     Gets the activity
        /// </summary>
        /// <param name="id">Activity id</param>
        /// <returns>Activity entity</returns>
        [HttpGet]
        [Route("{id}")]
        public Activity GetActivity(Guid id)
        {
            Activity activity = this.mediator.Send(new ActivityQuery { Id = id });

            if (activity == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Activity not found."));
            }

            return activity;
        }
    }
}
