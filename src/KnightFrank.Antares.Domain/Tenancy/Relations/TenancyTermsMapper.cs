namespace KnightFrank.Antares.Domain.Tenancy.Relations
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Domain.Tenancy.Commands;


    public class TenancyTermsMapper : ITenancyReferenceMapper<TenancyTerm>
    {
        public void ValidateAndAssign(TenancyCommandBase message, Tenancy entity)
        {
            message.Terms.Where(x => IsNewlyAddedTerm(x.Id)).ToList().ForEach(x =>
            {
                entity.Terms.Add(
                    new TenancyTerm
                    {
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Price = x.Price
                    });
            });
        }

        private static bool IsNewlyAddedTerm(Guid? id)
        {
            return !id.HasValue;
        }
    }
}