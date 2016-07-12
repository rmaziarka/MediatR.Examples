namespace KnightFrank.Antares.Domain.Tenancy.CommandHandlers.Relations
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    public class TenancyTermsMapper : ITenancyReferenceMapper<TenancyTerm>
    {
        public void ValidateAndAssign(TenancyCommandBase message, Tenancy entity)
        {
            if (!entity.Terms.Any())
            {
                entity.Terms.Add(new TenancyTerm());
            }

            TenancyTerm tenancyTerm = entity.Terms.First();

            tenancyTerm.StartDate = message.Term.StartDate;
            tenancyTerm.EndDate = message.Term.EndDate;
            tenancyTerm.AgreedRent = message.Term.Price;
        }
    }
}