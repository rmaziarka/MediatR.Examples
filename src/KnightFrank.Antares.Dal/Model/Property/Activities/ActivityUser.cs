namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.User;

    public class ActivityUser : BaseEntity
    {
        public Guid ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid UserTypeId { get; set; }
        public virtual EnumTypeItem UserType { get; set; }
        public DateTime? CallDate { get; set; }
    }
}