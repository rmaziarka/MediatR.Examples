namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using MediatR;
    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        
        public UpdateActivityCommandHandler(IGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public Guid Handle(UpdateActivityCommand message)
        {
            Activity activity = this.activityRepository.GetById(message.ActivityId);

            Mapper.Map(message, activity);

            this.activityRepository.Save();

            return activity.Id;
        }
    }
}