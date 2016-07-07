namespace KnightFrank.Antares.Dal.Model.Tenancy
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class TenancyContact : BaseEntity
    {
        public Guid TenancyId { get; set; }
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        public Guid ContactTypeId { get; set; }
        public virtual EnumTypeItem ContactType { get; set; }
    }
}
