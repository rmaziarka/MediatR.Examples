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

        // controller
        public managingDepartmentType: Dto.IEnumItem;
        public standardDepartmentType: Dto.IEnumItem;
        private activityEdit: Antares.Activity.ActivityEditController;

        constructor(
            private enumService: Services.EnumService) {

            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: Dto.IEnumDictionary) => {
            this.managingDepartmentType = this.getDepartmentTypeByCode(result, Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing]);
            this.standardDepartmentType = this.getDepartmentTypeByCode(result, Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard]);
        }

        private getDepartmentTypeByCode = (result: Dto.IEnumDictionary, code: string): Dto.IEnumItem => {
            var departmentTypes: Dto.IEnumItem[] = result[Dto.EnumTypeCode.ActivityDepartmentType];

            return departmentTypes.filter((department: Dto.IEnumItem) => {
                return department.code === code;
            })[0];
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