namespace KnightFrank.Antares.Domain.Resource.Queries
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class ResourceLocalisedQueryDomainValidator : AbstractValidator<ResourceLocalisedQuery>,
                                                         IDomainValidator<ResourceLocalisedQuery>
    {
        public ResourceLocalisedQueryDomainValidator(IGenericRepository<Locale> localeRepository)
        {
            this.RuleFor(x => x.IsoCode).SetValidator(new LocaleValidator(localeRepository));
        }
    }
}
