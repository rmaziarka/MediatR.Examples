/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class RequirementCardController {
        requirement: Business.Requirement;

        constructor(
            private $state: ng.ui.IStateService) {
        }

        navigateToRequirement = (requirement: Dto.IRequirement) =>{
            this.$state.go('app.requirement-view', { id : requirement.id });
        }
    }

    angular.module('app').controller('requirementCardController', RequirementCardController);
};