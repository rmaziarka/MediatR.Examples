namespace KnightFrank.Antares.Domain.AddressForm.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model.Address;

    public class IsAddressFormGloballyDefined : Specification<AddressForm>
    {
        public override Expression<Func<AddressForm, bool>> SatisfiedBy()
        {
            return af => !af.AddressFormEntityTypes.Any();
        }
    }
}