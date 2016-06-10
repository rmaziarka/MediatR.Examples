namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    public class UpdateActivityDepartment
    {
        public Guid DepartmentId { get; set; }

        public Guid DepartmentTypeId { get; set; }
    }
}
