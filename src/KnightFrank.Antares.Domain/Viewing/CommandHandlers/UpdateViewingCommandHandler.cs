namespace KnightFrank.Antares.Domain.Viewing.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Repository;
    using Commands;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class UpdateViewingCommandHandler : IRequestHandler<UpdateViewingCommand, Guid>
    {
        private readonly IGenericRepository<Viewing> viewingRepository;
        private readonly IEntityValidator entityValidator;
        private readonly ICollectionValidator collectionValidator;

        public UpdateViewingCommandHandler(
            IGenericRepository<Viewing> viewingRepository,
            IEntityValidator entityValidator,
            ICollectionValidator collectionValidator)
        {
            this.viewingRepository = viewingRepository;
            this.entityValidator = entityValidator;
            this.collectionValidator = collectionValidator;
        }


        public Guid Handle(UpdateViewingCommand message)
        {
            Viewing viewing = this.viewingRepository
                                  .GetWithInclude(
                                      x => x.Id == message.Id,
                                      x => x.Requirement.Contacts,
                                      x => x.Attendees)
                                  .SingleOrDefault();

            this.entityValidator.EntityExists(viewing, message.Id);

            // ReSharper disable once PossibleNullReferenceException
            IEnumerable<Guid> applicantIds = viewing.Requirement.Contacts.Select(x => x.Id);
            this.collectionValidator.CollectionContainsAll(applicantIds, message.AttendeesIds, ErrorMessage.Missing_Requirement_Attendees_Id);
            
            AutoMapper.Mapper.Map(message, viewing);

            List<Contact> attendees = viewing.Requirement.Contacts.Where(x => message.AttendeesIds.Contains(x.Id)).ToList();
            viewing.Attendees = attendees;
            
            this.viewingRepository.Save();

            return viewing.Id;
        }
    }
}
