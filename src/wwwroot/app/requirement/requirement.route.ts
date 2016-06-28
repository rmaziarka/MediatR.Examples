/// <reference path="../typings/_all.d.ts" />

module Antares.Requirement {
    import IRequirement = Antares.Common.Models.Dto.IRequirement;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;
    import ActivityEditConfig = Antares.Activity.IActivityEditConfig;
    var app: ng.IModule = angular.module('app.requirement');
    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.requirement-add', {
                url: '/requirement/add',
                template: '<requirement-add config="config" user-data="appVm.userData"></requirement-add>',
                controller: "RequirementRouteAddController",
                resolve: {
                    config: (configService: Services.ConfigService) => {
                        return configService.getRequirement(PageTypeEnum.Create,
                            null,
                            null);
                    }
                }
            })
            .state('app.requirement-view', {
                url: '/requirement/:id',
                template: "<requirement-view requirement='requirement' config='config'></requirement-view>",
                controller: "RequirementRouteViewController",
                resolve: {
                    requirement: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var requirementId: string = $stateParams['id'];
                        return dataAccessService.getRequirementResource().get({ id: requirementId }).$promise;
                    },
                    config: (requirement: IRequirement, configService: Services.ConfigService) => {
                        return configService.getRequirement(PageTypeEnum.Details,
                            requirement.requirementTypeId,
                            requirement);
                    }
                }
            });
    }
}