///<reference path="../../typings/_all.d.ts"/>

module Antares.Component {
    import Business = Common.Models.Business;

    export class OfferViewController {
        constructor(private $state: ng.ui.IStateService) {
        }

        navigateToActivity = (ativity: Business.Activity) =>{
            this.$state.go('app.activity-view', { id: ativity.id });
        }

        navigateToRequirement = (requirement: Business.Requirement) => {
            this.$state.go('app.requirement-view', { id: requirement.id});
        }
    }

    angular.module('app').controller('offerViewController', OfferViewController);
}