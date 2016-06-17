/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityEditController {
        public config: IActivityEditConfig;
        public activity: Business.Activity;
        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;
        private departmentsController: Antares.Attributes.ActivityDepartmentsEditControlController;

        private standardDepartmentType: Dto.IEnumTypeItem;

        private departmentErrorMessageCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.MESSAGE';
        private departmentErrorTitleCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.TITLE';

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService,
            private enumService: Services.EnumService,
            private kfMessageService: Services.KfMessageService) {

            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        private onEnumLoaded = (result: any) => {
            var departmentTypes: any = result[Dto.EnumTypeCode.ActivityDepartmentType];
            this.standardDepartmentType = <Dto.IEnumTypeItem>_.find(departmentTypes, (item: Dto.IEnumTypeItem) => {
                return item.code === Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard];
            });
        }

        public save() {
            var valid = this.anyNewDepartmentIsRelatedWithNegotiator();
            if (!valid) {
                this.kfMessageService.showErrorByCode(this.departmentErrorMessageCode, this.departmentErrorTitleCode);
                return;
            } 

            this.dataAccessService.getActivityResource()
                .update(new Business.UpdateActivityResource(this.activity))
                .$promise
                .then((activity: Dto.IActivity) =>{
                    this.$state.go('app.activity-view', activity);
                });
        }

        activityStatusChanged = (activityStatusId: string) => {
        }
        
        cancel() {
            this.$state.go('app.activity-view', { id: this.activity.id });
        }

        setDepartmentsEdit(departmentsController: Antares.Attributes.ActivityDepartmentsEditControlController) {
            this.departmentsController = departmentsController;
        }

        onNegotiatorAdded = (user: Dto.IUser) => {
            this.addDepartment(user.department);
        }

        public anyNewDepartmentIsRelatedWithNegotiator = () => {
            var newDepartments = this.activity.activityDepartments.filter((item: Business.ActivityDepartment) => { return !item.id });
            return _.all(newDepartments, (item) => this.departmentIsRelatedWithNegotiator(item.department));
        }

        public departmentIsRelatedWithNegotiator = (department: Business.Department) => {
            return this.activity.leadNegotiator.user.departmentId === department.id ||
                _.some(this.activity.secondaryNegotiator, (item) => item.user.departmentId === department.id);
        }


        private addDepartment(department: Business.Department) {
            if (!_.some(this.activity.activityDepartments, { 'departmentId': department.id })) {
                this.activity.activityDepartments.push(this.createActivityDepartment(department));
            }
        }

        private createActivityDepartment = (department: Dto.IDepartment) => {
            var activityDepartment = new Business.ActivityDepartment();
            activityDepartment.activityId = this.activity.id;
            activityDepartment.departmentId = department.id;
            activityDepartment.department = new Business.Department(<Dto.IDepartment>department);
            activityDepartment.departmentType = this.standardDepartmentType;
            activityDepartment.departmentTypeId = this.standardDepartmentType.id;

            return activityDepartment;
        }
    }

    angular.module('app').controller('ActivityEditController', ActivityEditController);
};