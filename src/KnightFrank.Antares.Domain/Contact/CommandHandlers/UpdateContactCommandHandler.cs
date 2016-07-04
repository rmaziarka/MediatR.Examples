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

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Guid>
    {
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IGenericRepository<User> userRepository;
        private readonly IGenericRepository<ContactUser> contactUserRepository;
        private readonly ICollectionValidator collectionValidator;
        private readonly IEntityValidator entityValidator;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public UpdateContactCommandHandler(
            IGenericRepository<Contact> contactRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<ContactUser> contactUserRepository,
            IEntityValidator entityValidator,
            ICollectionValidator collectionValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.contactRepository = contactRepository;
            this.collectionValidator = collectionValidator;
            this.entityValidator = entityValidator;
            this.userRepository = userRepository;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.contactUserRepository = contactUserRepository;
        }

        public Guid Handle(UpdateContactCommand command)
        {
            //Contact contact = this.contactRepository.GetById(command.Id);
            Contact contact = this.contactRepository.GetWithInclude(x => x.Id == command.Id,
                 x => x.ContactUsers)
                 .SingleOrDefault();

            this.entityValidator.EntityExists(contact, command.Id);

            List<ContactUser> commandNegotiators = this.ValidateAndRetrieveNegotiatorsFromCommand(command);

            Mapper.Map(command, contact);
            
            this.ValidateContactNegotiators(commandNegotiators, contact);
            this.UpdateContactNegotiators(commandNegotiators, contact);
           
            this.contactRepository.Save();

            return command.Id;   
        }

        private List<ContactUser> ValidateAndRetrieveNegotiatorsFromCommand(UpdateContactCommand command)
        {
            // Lead negotiator
            this.entityValidator.EntityExists<User>(command.LeadNegotiator.UserId);

            User leadNegotiator = this.GetUser(command.LeadNegotiator.UserId);
            var commandNegotiators = new List<ContactUser>
                                         {
                                             new ContactUser
                                                 {
                                                     UserId = leadNegotiator.Id,
                                                     User = leadNegotiator,
                                                     UserType = this.GetLeadNegotiatorUserType()
                                                 }
                                         };

            //Secondary negotiators
            this.entityValidator.EntitiesExist<User>(command.SecondaryNegotiators.Select(n => n.UserId).ToList());

            commandNegotiators.AddRange(
                command.SecondaryNegotiators.Select(
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

            // All negotirators
            this.collectionValidator.CollectionIsUnique(
                commandNegotiators.Select(n => n.UserId).ToList(),
                ErrorMessage.Negotiators_Not_Unique);

            return commandNegotiators;
        }

        private void ValidateContactNegotiators(List<ContactUser> commandNegotiators, Contact contact)
        {
            List<ContactUser> existingNegotiators = contact.ContactUsers.ToList();

            commandNegotiators.Where(n => IsNewlyAddedToExistingList(n, existingNegotiators))
                                                  .ToList();

            commandNegotiators.Where(n => IsUpdated(n, existingNegotiators))
                              .Select(n => new { newNegotiator = n, oldNegotiator = GetOldContactUser(n, existingNegotiators) })
                              .ToList();
        }

        private void UpdateContactNegotiators(List<ContactUser> commandNegotiators, Contact contact)
        {
            List<ContactUser> existingNegotiators = contact.ContactUsers.ToList();

            existingNegotiators.Where(n => IsRemovedFromExistingList(n, commandNegotiators))
                               .ToList()
                               .ForEach(n => this.contactUserRepository.Delete(n));

            commandNegotiators.Where(n => IsNewlyAddedToExistingList(n, existingNegotiators))
                              .ToList()
                              .ForEach(n => contact.ContactUsers.Add(n));

            commandNegotiators.Where(n => IsUpdated(n, existingNegotiators))
                              .Select(n => new { newNegotiator = n, oldNegotiator = GetOldContactUser(n, existingNegotiators) })
                              .ToList()
                              .ForEach(pair => UpdateNegotiator(pair.newNegotiator, pair.oldNegotiator));
        }

        private static bool IsRemovedFromExistingList(ContactUser existingContactUsers, IEnumerable<ContactUser> contactUsers)
        {
            return !contactUsers.Select(x => x.UserId).Contains(existingContactUsers.UserId);
        }

        private static bool IsNewlyAddedToExistingList(ContactUser contactUser, IEnumerable<ContactUser> existingContactUsers)
        {
            return !existingContactUsers.Select(x => x.UserId).Contains(contactUser.UserId);
        }

        private static bool IsUpdated(ContactUser contactUser, IEnumerable<ContactUser> existingContactUsers)
        {
            return !IsNewlyAddedToExistingList(contactUser, existingContactUsers);
        }

        private static ContactUser GetOldContactUser(ContactUser contactUser, IEnumerable<ContactUser> existingContactUsers)
        {
            return existingContactUsers.SingleOrDefault(x => x.UserId == contactUser.UserId);
        }

        private static void UpdateNegotiator(ContactUser newNegotiator, ContactUser oldNegotiator)
        {
            oldNegotiator.UserType = newNegotiator.UserType;
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
