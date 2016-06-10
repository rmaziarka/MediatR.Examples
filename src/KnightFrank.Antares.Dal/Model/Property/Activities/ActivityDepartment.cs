namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.User;

    public class ActivityDepartment : BaseEntity
    {
        public Guid ActivityId { get; set; }

        public virtual Activity Activity { get; set; }

        public Guid DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public Guid DepartmentTypeId { get; set; }

        public virtual EnumTypeItem DepartmentType { get; set; }
    }
}
