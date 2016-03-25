namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Web.Http;

    using Dal.Repository;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using MediatR;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    public class ActivitiesController : ApiController
    {
        private readonly IMediator mediator;
        private readonly IReadGenericRepository<Activity> activityRepository;

        /// <summary>
        ///     Contacts controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        /// <param name="activityRepository"></param>
        public ActivitiesController(IMediator mediator, IReadGenericRepository<Activity> activityRepository)
        {
            this.mediator = mediator;
            this.activityRepository = activityRepository;
        }

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="command">Contact entity</param>
        [HttpPost]
        public Activity CreateActivity([FromBody] CreateActivityCommand command)
        {
            Guid activityId = this.mediator.Send(command);
            // TODO return correct activity
            return new Activity { Id = activityId };
        }
    }
}
