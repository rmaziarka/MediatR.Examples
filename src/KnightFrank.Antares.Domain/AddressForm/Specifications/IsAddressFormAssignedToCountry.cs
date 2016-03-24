namespace KnightFrank.Antares.Domain.AddressForm.Specifications
{
    using System;
    using System.Linq.Expressions;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Address;

    public class IsAddressFormAssignedToCountry : Specification<AddressForm>
    {
        private readonly Guid countryId;

        public IsAddressFormAssignedToCountry(Guid countryId)
        {
            this.countryId = countryId;
        }

        public override Expression<Func<AddressForm, bool>> SatisfiedBy()
        {
            return af => af.CountryId == this.countryId;
        }
    }
}