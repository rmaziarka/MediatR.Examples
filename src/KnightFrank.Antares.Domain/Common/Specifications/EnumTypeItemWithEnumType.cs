namespace KnightFrank.Antares.Domain.Common.Specifications
{
    using System;
    using System.Linq.Expressions;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class EnumTypeItemWithEnumType : Specification<EnumTypeItem>
    {
        public Enums.EnumType EnumType { get; }

        public EnumTypeItemWithEnumType(Enums.EnumType enumType)
        {
            this.EnumType = enumType;
        }

        public override Expression<Func<EnumTypeItem, bool>> SatisfiedBy()
        {
            return x => x.EnumType.Code == this.EnumType.ToString();
        }
    }
}