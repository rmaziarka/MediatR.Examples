namespace KnightFrank.Antares.Domain.Contact.CommandHandlers
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Contact.Commands;

    using MediatR;

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Guid>
    {
        private readonly IGenericRepository<Contact> contactRepository;

        public UpdateContactCommandHandler(IGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Guid Handle(UpdateContactCommand command)
        {
            Contact contact = this.contactRepository.GetById(command.Id);
            
            // todo: ??? change to: IEntityMapper<Contact> data shaping ???
            Mapper.Map(command, contact);

            // todo: edit salutations
            // todo: edit negotiators

            this.contactRepository.Save();

            return command.Id;
        }
    }
}
