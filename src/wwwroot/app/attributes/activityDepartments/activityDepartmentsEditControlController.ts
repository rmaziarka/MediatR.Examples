/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes
{
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityDepartmentsEditControlController {
        // binding
        departmentIsRelatedWithNegotiator: (obj: { department: Business.Department }) => boolean;

        public activityId: string;
        public departments: Business.ActivityDepartment[];
        config: IActivityDepartmentsViewControlConfig;

        // controller
        public managingDepartmentType: Dto.IEnumItem;
        public standardDepartmentType: Dto.IEnumItem;
        private activityEdit: Activity.ActivityEditController;

        constructor(private enumProvider: Providers.EnumProvider) {
        }

        private $onInit = () => {
            this.managingDepartmentType = this.getDepartmentTypeByCode(Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing]);
            this.standardDepartmentType = this.getDepartmentTypeByCode(Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard]);
        }

        private getDepartmentTypeByCode = (code: string): Dto.IEnumItem => {
            //TODO: remove when mocking of enumProvider in testing is done
            if (this.enumProvider.enums) {
                var departmentTypes: Dto.IEnumItem[] = this.enumProvider.enums[Dto.EnumTypeCode.ActivityDepartmentType];

                return departmentTypes.filter((department: Dto.IEnumItem) =>{
                    return department.code === code;
                })[0];
            }
        }

        public deleteDepartment = (activityDepartment: Business.ActivityDepartment) => {
            if (!this.departmentIsRelatedWithNegotiator({ department: activityDepartment.department })) {
                _.remove(this.departments, (itm) => itm.department.id === activityDepartment.department.id);
            }
        }

        public isDeleteDisabled = (department: Business.Department) => {
            return this.departmentIsRelatedWithNegotiator({ department: department });
        }

        public setAsManagingDepartment = (activityDepartment: Business.ActivityDepartment) => {
            _.forEach(this.departments, (department) =>{
                department.departmentType = this.standardDepartmentType;
                department.departmentTypeId = this.standardDepartmentType.id;
            });

            activityDepartment.departmentType = this.managingDepartmentType;
            activityDepartment.departmentTypeId = this.managingDepartmentType.id;
        }
    }

    angular.module('app').controller('ActivityDepartmentsEditControlController', ActivityDepartmentsEditControlController);
};