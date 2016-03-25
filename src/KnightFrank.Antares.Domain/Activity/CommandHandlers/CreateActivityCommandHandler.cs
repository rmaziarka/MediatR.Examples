namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using MediatR;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;

        public CreateActivityCommandHandler(IGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public Guid Handle(CreateActivityCommand message)
        {
            var activity = AutoMapper.Mapper.Map<Activity>(message);
            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }
    }
}