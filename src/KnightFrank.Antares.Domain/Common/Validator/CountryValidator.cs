namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;

    public class CountryValidator : AbstractValidator<Guid>
    {
        private readonly IGenericRepository<Country> countryRepository;

        public CountryValidator(IGenericRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
            this.Custom(this.CountryExists);
        }

        private ValidationFailure CountryExists(Guid countryId)
        {
            bool countryExists = this.countryRepository.Any(x => x.Id.Equals(countryId));
            return countryExists
                       ? null
                       : new ValidationFailure(nameof(countryId), "Country does not exist.") { ErrorCode = "countryid_notexist" };
        }
    }
}
