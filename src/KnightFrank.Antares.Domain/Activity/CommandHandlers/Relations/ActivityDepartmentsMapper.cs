namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class ActivityDepartmentsMapper : IActivityReferenceMapper<ActivityDepartment>
    {
        private readonly IGenericRepository<Department> departmentRepository;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly ICollectionValidator collectionValidator;

        public ActivityDepartmentsMapper(IGenericRepository<Department> departmentRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository, ICollectionValidator collectionValidator)
        {
            this.departmentRepository = departmentRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.collectionValidator = collectionValidator;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            if (message.Departments.Count == 0)
            {
                throw new BusinessValidationException(ErrorMessage.Activity_Should_Have_Exactly_One_Managing_Department);
            }

            List<Guid> departmentsIds = message.Departments.Select(x => x.DepartmentId).ToList();
            this.collectionValidator.CollectionIsUnique(departmentsIds, ErrorMessage.Activity_Departments_Not_Unique);

            this.collectionValidator.CollectionContainsAll(
                this.departmentRepository.FindBy(x => departmentsIds.Contains(x.Id)).Select(x => x.Id).ToList(),
                departmentsIds,
                ErrorMessage.Missing_Activity_Departments_Id);

            EnumTypeItem managingDepartmentType = this.GetManagingDepartmentType();
            int managingDepartmentsCount = message.Departments.Count(x => x.DepartmentTypeId == managingDepartmentType.Id);

            if (managingDepartmentsCount != 1)
            {
                throw new BusinessValidationException(ErrorMessage.Activity_Should_Have_Exactly_One_Managing_Department);
            }

            foreach (ActivityDepartment departmentToDelete in activity.ActivityDepartments.Where(x => departmentsIds.Contains(x.DepartmentId) == false).ToList())
            {
                activity.ActivityDepartments.Remove(departmentToDelete);
            }

            foreach (UpdateActivityDepartment messageDepartment in message.Departments)
            {
                ActivityDepartment existingDepartment =
                    activity.ActivityDepartments.SingleOrDefault(u => u.DepartmentId == messageDepartment.DepartmentId);
                if (existingDepartment == null)
                {
                    existingDepartment = new ActivityDepartment { DepartmentId = messageDepartment.DepartmentId };
                    activity.ActivityDepartments.Add(existingDepartment);
                }

                existingDepartment.DepartmentTypeId = messageDepartment.DepartmentTypeId;
            }
        }

        private EnumTypeItem GetManagingDepartmentType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityDepartmentType.Managing.ToString()).Single();
        }
    }
}
