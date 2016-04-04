namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using MediatR;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<Ownership> ownershipRepository;


        public CreateActivityCommandHandler(IGenericRepository<Activity> activityRepository, IGenericRepository<Ownership> ownershipRepository)
        {
            this.activityRepository = activityRepository;
            this.ownershipRepository = ownershipRepository;
        }

        public Guid Handle(CreateActivityCommand message)
        {
            var activity = AutoMapper.Mapper.Map<Activity>(message);

            DateTime? noSellDate = null;
            Ownership ownerships = this.ownershipRepository.GetWithInclude(o => o.PropertyId == activity.PropertyId && o.SellDate == noSellDate, p => p.Contacts).SingleOrDefault();
            
            activity.Contacts = ownerships?.Contacts;
            
            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }
    }
}