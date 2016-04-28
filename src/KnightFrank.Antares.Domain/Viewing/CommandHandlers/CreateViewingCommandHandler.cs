namespace KnightFrank.Antares.Domain.Viewing.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Repository;
    using Commands;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class CreateViewingCommandHandler : IRequestHandler<CreateViewingCommand, Guid>
    {
        private readonly IGenericRepository<Viewing> viewingRepository;

        private readonly IGenericRepository<Requirement> requirementRepository;

        private readonly IEntityValidator entityValidator;

        public CreateViewingCommandHandler(IGenericRepository<Viewing> viewingRepository,
            IEntityValidator entityValidator,
            IGenericRepository<Requirement> requirementRepository)
        {
            this.viewingRepository = viewingRepository;
            this.entityValidator = entityValidator;
            this.requirementRepository = requirementRepository;
        }

        public Guid Handle(CreateViewingCommand message)
        {
            this.entityValidator.ThrowExceptionIfNotExist<Activity>(message.ActivityId);

            Requirement requirement = this.requirementRepository.GetById(message.RequirementId);
            if (requirement == null)
            {
                throw new BusinessValidationException(BusinessValidationMessage.CreateEntityNotExistMessage(typeof(Requirement).ToString(), message.RequirementId));
            }

            var applicantIds = requirement.Contacts.Select(y => y.Id);

            if (!message.AttendeesIds.All(x => applicantIds.Contains(x)))
            {
                throw new BusinessValidationException(new BusinessValidationMessage("", ""));
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
