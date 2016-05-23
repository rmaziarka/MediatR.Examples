/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityEditController {
        public activity: Business.Activity;
        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService){
        }

        public save() {
            this.dataAccessService.getActivityResource()
                .update(new Business.UpdateActivityResource(this.activity))
                .$promise
                .then((activity: Dto.IActivity) => {
                    this.$state.go('app.activity-view', activity);
                });
        }
        
        cancel() {
            this.$state.go('app.activity-view', { id: this.activity.id });
        }
    }

    angular.module('app').controller('ActivityEditController', ActivityEditController);
};