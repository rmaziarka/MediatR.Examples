/// <reference path="../typings/_all.d.ts" />

module Antares.Requirement {
    var app: ng.IModule = angular.module('app.requirement');
    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.requirement-add', {
                url: '/requirement/add',
                template: '<requirement-add user-data="appVm.userData"></requirement-add>'
            })
            .state('app.requirement-view', {
                url: '/requirement/:id',
                template: "<requirement-view requirement='requirement'></requirement-view>",
                controller: "RequirementRouteViewController",
                resolve: {
                    requirement: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var requirementId: string = $stateParams['id'];
                        return dataAccessService.getRequirementResource().get({ id: requirementId }).$promise;
                    }
                }
            });
    }
}