/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyEditController {
        tenancy: TenancyEditModel;

        constructor(private $state: ng.ui.IStateService) {}

        navigateToActivity = (ativity: Business.Activity) => {
            this.$state.go('app.activity-view', { id: ativity.id });
        }

        navigateToRequirement = (requirement: Tenancy.RequirementPreviewEditModel) => {
            this.$state.go('app.requirement-view', { id: requirement.id });
        }

        cancel() {
            this.$state.go('app.activity-view', {id: this.tenancy.activity.id});
        }
    }

    angular.module('app').controller('TenancyEditController', TenancyEditController);
}