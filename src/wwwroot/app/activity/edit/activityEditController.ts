/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import DepartmentsController = Antares.Common.Component.DepartmentsController;

    export class ActivityEditController {
        public config: IActivityEditConfig;
        public activity: Business.Activity;
        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;
        private departmentsController: DepartmentsController;

        private departmentErrorMessageCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.MESSAGE';
        private departmentErrorTitleCode: string = 'DEPARTMENTS.COMMON.NEWDEPARTMENTISNOTRELATEDWITHNEGOTIATORERROR.TITLE';

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService,
            private kfMessageService: Services.KfMessageService) {
        }

        public save() {
            var valid = this.departmentsController.anyNewDepartmentIsRelatedWithNegotiator();
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

        setDepartmentsEdit(departmentsController: DepartmentsController){
            this.departmentsController = departmentsController;
        }

    }

    angular.module('app').controller('ActivityEditController', ActivityEditController);
};