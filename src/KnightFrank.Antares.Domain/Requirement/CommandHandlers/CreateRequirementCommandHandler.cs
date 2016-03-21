namespace KnightFrank.Antares.Domain.Requirement.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Model;
    using Dal.Repository;
    using Commands;

    using MediatR;

    public class CreateRequirementCommandHandler : IRequestHandler<CreateRequirementCommand, Guid>
    {
        private readonly IGenericRepository<Requirement> requirementRepository;

        private readonly IGenericRepository<Contact> contactRepository;

        public CreateRequirementCommandHandler(IGenericRepository<Requirement> requirementRepository, IGenericRepository<Contact> contactRepository)
        {
            this.requirementRepository = requirementRepository;
            this.contactRepository = contactRepository;
        }

        public Guid Handle(CreateRequirementCommand message)
        {
            var requirement = AutoMapper.Mapper.Map<Requirement>(message);

            List<Guid> ids = message.Contacts.Select(x => x.Id).ToList();
            List<Contact> existingContacts = this.contactRepository.FindBy(x => ids.Any(id => id == x.Id)).ToList();
            requirement.Contacts = existingContacts;
            requirement.CreateDate = DateTime.UtcNow;

            this.requirementRepository.Add(requirement);
            this.requirementRepository.Save();

            return requirement.Id;
        }
    }
}
