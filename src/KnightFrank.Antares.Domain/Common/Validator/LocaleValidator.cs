namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;

    public class LocaleValidator : AbstractValidator<string>
    {
        private readonly IGenericRepository<Locale> localeRepository;

        public LocaleValidator(IGenericRepository<Locale> localeRepository)
        {
            this.localeRepository = localeRepository;

            this.Custom(this.LocalesExistValidator);
        }

        private ValidationFailure LocalesExistValidator(string isoCode)
        {
            bool isExistLocale = this.localeRepository.Any(l => l.IsoCode == isoCode);
            return isExistLocale
                       ? null
                       : new ValidationFailure(nameof(isoCode), "IsoCode is invalid.") { ErrorCode = "isocodeinvalid_error" };
        }
    }
}
