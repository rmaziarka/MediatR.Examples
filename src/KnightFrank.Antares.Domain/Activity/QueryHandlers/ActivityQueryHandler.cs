namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;

    using MediatR;

    public class ActivityQueryHandler : IRequestHandler<ActivityQuery, Activity>
    {
        private readonly IReadGenericRepository<Activity> activityRepository;

        public ActivityQueryHandler(IReadGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public Activity Handle(ActivityQuery query)
        {
            Activity result =
                this.activityRepository.Get()
                    .Include(a => a.Property)
                    .Include(a => a.Property.PropertyType)
                    .Include(a => a.Property.Address)
                    .Include(a => a.Contacts)
                    .Include(a => a.Attachments)
                    .Include(a => a.Attachments.Select(at => at.User))
                    .Include(a => a.Viewings)
                    .Include(a => a.Viewings.Select(v => v.Attendees))
                    .Include(a => a.Viewings.Select(v => v.Negotiator))
                    .Include(a => a.Viewings.Select(v => v.Requirement))
                    .Include(a => a.Viewings.Select(v => v.Requirement.Contacts))
                    .Include(a => a.ActivityType)
                    .Include(a => a.ActivityUsers)
                    .Include(a => a.ActivityUsers.Select(an => an.User))
                    .Include(a => a.ActivityUsers.Select(an => an.UserType))
                    .Include(a => a.ActivityDepartments)
                    .Include(a => a.ActivityDepartments.Select(an => an.Department))
                    .Include(a => a.ActivityDepartments.Select(an => an.DepartmentType))
                    .Include(a => a.Offers)
                    .Include(a => a.Offers.Select(v => v.Requirement))
                    .Include(a => a.Offers.Select(v => v.Negotiator))
                    .Include(a => a.Offers.Select(v => v.Requirement.Contacts))
                    .Include(a => a.AppraisalMeetingAttendees.Select(aa => aa.User))
                    .Include(a => a.AppraisalMeetingAttendees.Select(aa => aa.Contact))
                    .Include(a => a.ChainTransactions)
                    .Include(a => a.ChainTransactions.Select(c => c.AgentCompany))
                    .Include(a => a.ChainTransactions.Select(c => c.AgentContact))
                    .Include(a => a.ChainTransactions.Select(c => c.AgentUser))
                    .Include(a => a.ChainTransactions.Select(c => c.Property.Address))
                    .Include(a => a.ChainTransactions.Select(c => c.Property.Ownerships))
                    .Include(a => a.ChainTransactions.Select(c => c.SolicitorCompany))
                    .Include(a => a.ChainTransactions.Select(c => c.SolicitorContact))
                    .Include(a => a.ChainTransactions.Select(c => c.SolicitorContact))
                    .SingleOrDefault(a => a.Id == query.Id);

            return result;
        }
    }
}
