namespace KnightFrank.Antares.Domain.Viewing.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Repository;
    using Commands;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;

    public class CreateViewingCommandHandler : IRequestHandler<CreateViewingCommand, Guid>
    {
        private readonly IGenericRepository<Viewing> viewingRepository;

        private readonly IGenericRepository<Contact> contactRepository;

        private readonly IDomainValidator<CreateViewingCommand> createViewingCommandDomainValidator;

        public CreateViewingCommandHandler(IGenericRepository<Viewing> viewingRepository, IGenericRepository<Contact> contactRepository, IDomainValidator<CreateViewingCommand>  createViewingCommandDomainValidator)
        {
            this.viewingRepository = viewingRepository;
            this.contactRepository = contactRepository;
            this.createViewingCommandDomainValidator = createViewingCommandDomainValidator;
        }

        public Guid Handle(CreateViewingCommand message)
        {
            ValidationResult validationResult = this.createViewingCommandDomainValidator.Validate(message);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var viewing = AutoMapper.Mapper.Map<Viewing>(message);

            List<Contact> existingAttendees = this.contactRepository.FindBy(x => message.AttendeesIds.Any(id => id == x.Id)).ToList();
            viewing.Attendees = existingAttendees;

            this.viewingRepository.Add(viewing);
            this.viewingRepository.Save();

            return viewing.Id;
        }
    }
}
