namespace KnightFrank.Antares.Domain.Enum.QueryHandlers
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Enum.Queries;

    public class EnumLocalisedQueryDomainValidator : AbstractValidator<EnumLocalisedQuery>, IDomainValidator<EnumLocalisedQuery>
    {
        public EnumLocalisedQueryDomainValidator(IGenericRepository<Locale> localeRepository)
        {
            this.RuleFor(x => x.IsoCode).SetValidator(new LocaleValidator(localeRepository));
        }
    }
}
