namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    /// <summary>
    ///     Controller class for activities
    /// </summary>
    [RoutePrefix("api/activities")]
    public class ActivitiesController : ApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Activities controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        public ActivitiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///     Creates the activity
        /// </summary>
        /// <param name="command">Activity data to create</param>
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

        /// <summary>
        ///     Gets all matching activities for requirement
        /// </summary>
        /// <returns>Activity entity collection</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<ActivitiesQueryResult> GetMatchingActivitiesForRequirement([FromUri(Name = "")]ActivitiesQuery query)
        {
            return this.mediator.Send(query ?? new ActivitiesQuery());
        }

        /// <summary>
        ///     Updates the activity
        /// </summary>
        /// <param name="command">Activity data to update</param>
        /// <returns>Activity entity</returns>
        [HttpPut]
        [Route("")]
        public Activity UpdateActivity(UpdateActivityCommand command)
        {
            Guid activityId = this.mediator.Send(command);

            return this.GetActivity(activityId);
        }

        /// <summary>
        /// Gets the activity types.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>List of activity types</returns>
        [HttpGet]
        [Route("types")]
        public IEnumerable<ActivityTypeQueryResult> GetActivityTypes([FromUri(Name = "")]ActivityTypeQuery query)
        {
            return this.mediator.Send(query);
        }
    }
}
