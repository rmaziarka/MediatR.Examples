namespace KnightFrank.Antares.Domain.Tenancy.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    public class TenancyContactMapper : IReferenceMapper<CreateTenancyCommand, Tenancy, Contact> 
    {
        private readonly IEntityValidator entityValidator;
        private readonly ICollectionValidator collectionValidator;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public TenancyContactMapper(IEntityValidator entityValidator,
            ICollectionValidator collectionValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.entityValidator = entityValidator;
            this.collectionValidator = collectionValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
        }

        public void ValidateAndAssign(CreateTenancyCommand message, Tenancy tenancy)
        {
            this.ValidateAndAssign(message.LandlordContacts, tenancy, TenancyContactType.Landlord);
            this.ValidateAndAssign(message.TenantContacts, tenancy, TenancyContactType.Tenant);
        }

        private void ValidateAndAssign(IList<Guid> contactIds, Tenancy tenancy, TenancyContactType tenancyContactType)
        {
            if (!contactIds.Any())
            {
                return;
            }

            this.collectionValidator.CollectionIsUnique(contactIds, ErrorMessage.Tenancy_Contacts_Not_Unique);
            this.entityValidator.EntitiesExist<Contact>(contactIds);

            EnumTypeItem contactEnumTypeItem = this.GetTenancyType(tenancyContactType);

            foreach (Guid contactId in contactIds.Where(x => IsNewlyAddedUser(x, tenancy.Contacts, contactEnumTypeItem)))
            {
                tenancy.Contacts.Add(new TenancyContact
                {
                    ContactId = contactId,
                    ContactTypeId = contactEnumTypeItem.Id
                });
            }
        }

        private EnumTypeItem GetTenancyType(TenancyContactType tenancyContactType)
        {
            return this.enumTypeItemRepository.FindBy(x => x.Code == tenancyContactType.ToString() && x.EnumType.Code == Common.Enums.EnumType.TenancyContactType.ToString()).Single();
        }

        private static bool IsNewlyAddedUser(Guid id, ICollection<TenancyContact> tenantContacts, EnumTypeItem tenancyContactType)
        {
            return !tenantContacts.Where(x => x.ContactTypeId == tenancyContactType.Id)
                                     .Select(x => x.ContactId)
                                     .Contains(id);
        }
    }
}