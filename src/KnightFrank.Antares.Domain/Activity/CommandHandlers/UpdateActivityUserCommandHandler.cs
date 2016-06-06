namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    public class UpdateActivityUserCommandHandler : IRequestHandler<UpdateActivityUserCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;

        private readonly IGenericRepository<ActivityUser> activityUserRepository;

        private readonly IEntityValidator entityValidator;

        public UpdateActivityUserCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<ActivityUser> activityUserRepository,
            IEntityValidator entityValidator)
        {
            this.activityRepository = activityRepository;
            this.activityUserRepository = activityUserRepository;
            this.entityValidator = entityValidator;
        }

        public Guid Handle(UpdateActivityUserCommand message)
        {
            ActivityUser activityUser =
                this.activityUserRepository.GetWithInclude(x => x.Id == message.Id, x => x.UserType).SingleOrDefault();

            this.entityValidator.EntityExists(activityUser, message.Id);

            Activity activity = this.activityRepository.GetById(message.ActivityId);

            this.entityValidator.EntityExists(activity, message.ActivityId);

            this.ValidateActivityUserAssignToActivity(activityUser, activity);
            this.ValidateActivityUserCallDate(activityUser, message.CallDate);

            activityUser.CallDate = message.CallDate.Value.Date;

            this.activityUserRepository.Save();

            return activityUser.Id;
        }

        private void ValidateActivityUserAssignToActivity(ActivityUser activityUser, Activity activity)
        {
            if (activityUser.ActivityId != activity.Id)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityUser_Is_Assigned_To_Other_Activity);
            }
        }

        private void ValidateActivityUserCallDate(ActivityUser activityUser, DateTime? callDate)
        {
            if (activityUser.UserType.Code == EnumTypeItemCode.LeadNegotiator && callDate == null)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityUser_CallDate_Is_Required);
            }
        }
    }
}