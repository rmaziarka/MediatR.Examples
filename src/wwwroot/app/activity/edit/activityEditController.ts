/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityEditController {
        public activity: Business.Activity;
        public activityStatuses: any[];

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService){

            this.dataAccessService.getEnumResource()
                .get({ code: 'ActivityStatus' })
                .$promise.then(this.onActivityStatusLoaded);
        }

        onActivityStatusLoaded = (result: any) => {
            this.activityStatuses = result.items;
        }

        public save() {
            this.dataAccessService.getActivityResource()
                .update(new Business.UpdateActivityResource(this.activity))
                .$promise
                .then((activity: Dto.IActivity) => {
                    this.$state.go('app.activity-view', activity);
                });
        }
    }

    angular.module('app').controller('ActivityEditController', ActivityEditController);
};