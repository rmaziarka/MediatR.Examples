/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import RequirementViewConfig = Requirement.IRequirementViewConfig;

    export class RequirementRouteAddController {

        constructor(private $scope: ng.IScope, config: RequirementViewConfig, private latestViewsProvider: LatestViewsProvider) {
            $scope['config'] = config;
        }
    }

    angular.module('app').controller('RequirementRouteAddController', RequirementRouteAddController);
};