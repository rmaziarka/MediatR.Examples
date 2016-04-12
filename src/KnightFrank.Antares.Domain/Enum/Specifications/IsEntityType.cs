namespace KnightFrank.Antares.Domain.Enum.Specifications
{
    using System;
    using System.Linq.Expressions;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class IsEntityType : Specification<EnumTypeItem>
    {
        public override Expression<Func<EnumTypeItem, bool>> SatisfiedBy()
        {
            return x => x.EnumType.Code == "EntityType";
        }
    }
}