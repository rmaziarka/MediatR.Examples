namespace KnightFrank.Antares.Domain.Common.Specifications
{
    using System;
    using System.Linq.Expressions;

    using global::Domain.Seedwork.Specification;

    using Dal.Model.Common;

    public class IsLocaled<T> : Specification<T> where T: class, ILocaled
    {
        private readonly string isoCode;

        public IsLocaled(string isoCode)
        {
            this.isoCode = isoCode;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return l => l.Locale.IsoCode == this.isoCode;
        }
    }
}