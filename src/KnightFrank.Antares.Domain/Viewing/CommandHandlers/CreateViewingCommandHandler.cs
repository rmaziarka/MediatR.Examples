namespace KnightFrank.Antares.Domain.Viewing.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Repository;
    using Commands;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class CreateViewingCommandHandler : IRequestHandler<CreateViewingCommand, Guid>
    {
        private readonly IGenericRepository<Viewing> viewingRepository;

        private readonly IGenericRepository<Requirement> requirementRepository;

        private readonly IReadGenericRepository<User> userRepository;

        private readonly IEntityValidator entityValidator;

        public CreateViewingCommandHandler(IGenericRepository<Viewing> viewingRepository,
            IEntityValidator entityValidator,
            IGenericRepository<Requirement> requirementRepository,
            IReadGenericRepository<User> userRepository)
        {
            this.viewingRepository = viewingRepository;
            this.entityValidator = entityValidator;
            this.userRepository = userRepository;
            this.requirementRepository = requirementRepository;
        }


        public Guid Handle(CreateViewingCommand message)
        {
            this.entityValidator.EntityExists<Activity>(message.ActivityId);
            //TODO: Remove after users management is implementsd
            message.NegotiatorId = userRepository.Get().FirstOrDefault().Id;

            Requirement requirement = this.requirementRepository.GetWithInclude(r => r.Id == message.RequirementId, r => r.Contacts).SingleOrDefault();

            this.entityValidator.EntityExists(requirement, message.RequirementId);

            IEnumerable<Guid> applicantIds = requirement.Contacts.Select(y => y.Id);

            if (!message.AttendeesIds.All(x => applicantIds.Contains(x)))
            {
                throw new BusinessValidationException(ErrorMessage.Missing_Applicant_Id);
            }

            List<Contact> existingAttendees = requirement.Contacts.Where(x => message.AttendeesIds.Contains(x.Id)).ToList();

            var viewing = AutoMapper.Mapper.Map<Viewing>(message);

            viewing.Attendees = existingAttendees;

            this.viewingRepository.Add(viewing);
            this.viewingRepository.Save();

            return viewing.Id;
        }
    }
}
