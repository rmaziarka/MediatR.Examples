namespace KnightFrank.Antares.Domain.AddressForm.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model.Address;

    public class IsAddressFormAssignedToEntityType : Specification<AddressForm>
    {
        private readonly Guid enumTypeItemId;

        public IsAddressFormAssignedToEntityType(Guid enumTypeItemId)
        {
            this.enumTypeItemId = enumTypeItemId;
        }

        public override Expression<Func<AddressForm, bool>> SatisfiedBy()
        {
            return af => af.AddressFormEntityTypes.Any(afet => afet.EnumTypeItemId == this.enumTypeItemId);
        }
    }
}