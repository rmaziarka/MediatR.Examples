namespace KnightFrank.Antares.Domain.Common.Specifications
{
    using System;
    using System.Linq.Expressions;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model;
    public class HasId<T> : Specification<T> where T : BaseEntity
    {
        private readonly Guid id;

        public HasId(Guid id)
        {
            this.id = id;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return x => x.Id == this.id;
        }
    }
}