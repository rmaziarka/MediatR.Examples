namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Resource;

    public class PropertyTypeDefinition : BaseEntity
    {
        public Guid PropertyTypeId { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }
        public Guid DivisionId { get; set; }
        public virtual EnumTypeItem Division { get; set; }
        public short Order { get; set; }
    }
}
