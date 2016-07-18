namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Api;
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Portal;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Domain.Attachment.Queries;

    using MediatR;

    /// <summary>
    ///     Controller class for activities
    /// </summary>
    [RoutePrefix("api/activities")]
    public class ActivitiesController : ApiController
    {
        private readonly IMediator mediator;
        private readonly IGenericRepository<User> userRepository;

        /// <summary>
        ///     Activities controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        /// <param name="userRepository"></param>
        public ActivitiesController(IMediator mediator, IGenericRepository<User> userRepository)
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Gets the portals.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("portals")]
        public IEnumerable<Portal> GetPortals([FromUri]ActivityPortalsQuery query)
        {
            query = query ?? new ActivityPortalsQuery();
            IEnumerable<Portal> result = this.mediator.Send(query);
            return result;
        }

        /// <summary>
        ///     Creates the activity
        /// </summary>
        /// <param name="command">Activity data to create</param>
        [HttpPost]
        [Route("")]
        [DataShaping]
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
        [DataShaping]
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
        ///     Gets all activities list
        /// </summary>
        /// <returns>Activity entity collection</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<ActivitiesQueryResult> GetActivities([FromUri(Name = "")]ActivitiesFilterQuery query)
        {
            if (query == null)
                query = new ActivitiesFilterQuery();
            return this.mediator.Send(query);
        }

        /// <summary>
        ///     Updates the activity
        /// </summary>
        /// <param name="command">Activity data to update</param>
        /// <returns>Activity entity</returns>
        [HttpPut]
        [Route("")]
        [DataShaping]
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

        /// <summary>
        ///     Creates the attachment.
        /// </summary>
        /// <param name="id">Activity Id</param>
        /// <param name="command">Attachment data</param>
        /// <returns>Created attachment</returns>
        [HttpPost]
        [Route("{id}/attachments")]
        public Attachment CreateAttachment(Guid id, CreateActivityAttachmentCommand command)
        {
            // User id is mocked.
            // TODO Set correct user id from header.
            if (command.Attachment != null)
            {
                command.Attachment.UserId = this.userRepository.FindBy(u => true).First().Id;
            }

            command.EntityId = id;
            Guid attachmentId = this.mediator.Send(command);

            var attachmentQuery = new AttachmentQuery { Id = attachmentId };
            return this.mediator.Send(attachmentQuery);
        }

        /// <summary>
        ///     Updates the activity user call date
        /// </summary>
        /// <param name="id">Activity Id</param>
        /// <param name="command">ActivityUser data to update</param>
        /// <returns>ActivityUser entity</returns>
        [HttpPut]
        [Route("{id}/negotiators")]
        public ActivityUser UpdateActivityUser(Guid id, UpdateActivityUserCommand command)
        {
            command.ActivityId = id;
            Guid activityUserId = this.mediator.Send(command);

            return this.GetActivityUser(id, activityUserId);
        }

        private ActivityUser GetActivityUser(Guid activityId, Guid activityUserId)
        {
            ActivityUser activityUser = this.mediator.Send(new ActivityUserQuery { Id = activityUserId, ActivityId = activityId });

            if (activityUser == null)
            {
                throw new HttpResponseException(
                    this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "ActivityUser not found."));
            }

            return activityUser;
        }
    }
}
