/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import RequirementViewConfig = Requirement.IRequirementViewConfig;

    export class RequirementRouteViewController {

        constructor(private $scope: ng.IScope, requirement: Dto.IRequirement, config: RequirementViewConfig, private latestViewsProvider: LatestViewsProvider) {
            var requirementViewModel = new Business.RequirementViewModel(requirement);

            $scope['requirement'] = requirementViewModel;
            $scope['config'] = config;
            this.saveRecentViewedRequirement(requirementViewModel.id);
        }

        private saveRecentViewedRequirement(requirementId: string){
            this.latestViewsProvider.addView({
                entityId: requirementId,
                entityType : EntityType.Requirement
            });
        }
    }

    angular.module('app').controller('RequirementRouteViewController', RequirementRouteViewController);
};