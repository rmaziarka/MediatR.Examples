/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component
{
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    import ActivityEditController = Antares.Activity.ActivityEditController;

    export class DepartmentsController {
        public activityId: string;
        public departments: Business.ActivityDepartment[];
        public leadNegotiator: Business.ActivityUser;
        public secondaryNegotiators: Business.ActivityUser[];

        public managingDepartmentType: Dto.IEnumTypeItem;
        public standardDepartmentType: Dto.IEnumTypeItem;
        private activityEdit: ActivityEditController;

        constructor(
            private enumService: Services.EnumService) {

            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: any) => {
            var departmentTypes: any = result[Dto.EnumTypeCode.ActivityDepartmentType];
            this.managingDepartmentType = <Dto.IEnumTypeItem>_.find(departmentTypes, { 'code': Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing] });
            this.standardDepartmentType = <Dto.IEnumTypeItem>_.find(departmentTypes, { 'code': Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard] });
        }

        public deleteDepartment = (activityDepartment: Business.ActivityDepartment) => {
            if (!this.departmentIsRelatedWithNegotiator(activityDepartment.department)) {
                _.remove(this.departments, (itm) => itm.department.id === activityDepartment.department.id);
            }
        }

        public setAsManagingDepartment = (activityDepartment: Business.ActivityDepartment) => {
            _.forEach(this.departments, (department) =>{
                department.departmentType = this.standardDepartmentType;
                department.departmentTypeId = this.standardDepartmentType.id;
            });

            activityDepartment.departmentType = this.managingDepartmentType;
            activityDepartment.departmentTypeId = this.managingDepartmentType.id;
        }

        public departmentIsRelatedWithNegotiator = (department: Business.Department) =>{
            return this.leadNegotiator.user.departmentId === department.id ||
                _.some(this.secondaryNegotiators, (item) => item.user.departmentId === department.id);
        }

        public anyNewDepartmentIsRelatedWithNegotiator = () => {
            var newDepartments = this.departments.filter((item: Business.ActivityDepartment) => { return !item.id });
            return _.all(newDepartments, (item) => this.departmentIsRelatedWithNegotiator(item.department));
        }

        $onInit = () =>{
            this.activityEdit.setDepartmentsEdit(this);
        }
    }

    angular.module('app').controller('DepartmentsController', DepartmentsController);
};