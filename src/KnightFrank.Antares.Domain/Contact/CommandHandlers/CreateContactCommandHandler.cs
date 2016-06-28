namespace KnightFrank.Antares.Domain.Contact.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Contact.Commands;

    using MediatR;

    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Guid>
    {
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<User> userRepository;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly ICollectionValidator collectionValidator;

        public CreateContactCommandHandler(IGenericRepository<Contact> contactRepository,
            IGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository,
            ICollectionValidator collectionValidator
            )
        {
            this.contactRepository = contactRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.collectionValidator = collectionValidator;
        }

        public Guid Handle(CreateContactCommand message)
        {

            List<ContactUser> commandNegotiators = this.ValidateAndRetrieveNegotiatorsFromCommand(message);

            var contact = Mapper.Map<Contact>(message);
            this.UpdateContactNegotiators(commandNegotiators, contact);

            this.contactRepository.Add(contact);
            this.contactRepository.Save();

            return contact.Id;
        }

        private List<ContactUser> ValidateAndRetrieveNegotiatorsFromCommand(CreateContactCommand message)
        {
            // Lead negotiator
            this.entityValidator.EntityExists<User>(message.LeadNegotiator.UserId);

            User leadNegotiator = this.GetUser(message.LeadNegotiator.UserId);
            var commandNegotiators = new List<ContactUser>
            {
                new ContactUser
                {
                    UserId = leadNegotiator.Id,
                    User = leadNegotiator,
                    UserType = this.GetLeadNegotiatorUserType(),
                }
            };

            //Secondary negotiators
            this.entityValidator.EntitiesExist<User>(message.SecondaryNegotiators.Select(n => n.UserId).ToList());

            commandNegotiators.AddRange(
                message.SecondaryNegotiators.Select(
                    n =>
                    {
                        User user = this.GetUser(n.UserId);
                        return new ContactUser
                        {
                            UserId = user.Id,
                            User = user,
                            UserType = this.GetSecondaryNegotiatorUserType()
                        };
                    }));

            // All negotiators
            this.collectionValidator.CollectionIsUnique(
                commandNegotiators.Select(n => n.UserId).ToList(),
                ErrorMessage.Negotiators_Not_Unique);

            return commandNegotiators;
        }

        private void UpdateContactNegotiators(List<ContactUser> commandNegotiators, Contact contact)
        {
            commandNegotiators.ForEach(n => contact.ContactUsers.Add(n));
        }

        private User GetUser(Guid userId)
        {
            return this.userRepository.GetWithInclude(x => x.Id == userId).Single();
        }

        private EnumTypeItem GetLeadNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == UserType.LeadNegotiator.ToString()).Single();
        }

        private EnumTypeItem GetSecondaryNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == UserType.SecondaryNegotiator.ToString()).Single();
        }
    }
}
