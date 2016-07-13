namespace KnightFrank.Antares.Dal.Model.Contacts
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.User;

    public class ContactUser : BaseEntity
    {
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid UserTypeId { get; set; }
        public virtual EnumTypeItem UserType { get; set; }
     }
}