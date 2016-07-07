/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class RequirementCardController {
        requirement: Business.Requirement;

        constructor(
            private $state: ng.ui.IStateService,
            private appConfig: Common.Models.IAppConfig,
            private $window: ng.IWindowService) {
        }

        navigateToRequirement = (requirement: Dto.IRequirement) => {
            var requirementViewUrl = this.appConfig.appRootUrl + this.$state.href('app.requirement-view', { id: requirement.id }, { absolute: false });
            this.$window.open(requirementViewUrl, '_blank');
        }
    }

    angular.module('app').controller('requirementCardController', RequirementCardController);
};