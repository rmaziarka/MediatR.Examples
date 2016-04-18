/// <reference path="../typings/_all.d.ts" />

module Antares.Requirement {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

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
                controller: ($scope: ng.IScope, requirement: Dto.IRequirement) => {
                    var requirementViewModel = new Business.Requirement(<Dto.IRequirement>requirement);

                    $scope['requirement'] = requirementViewModel;
                },
                resolve: {
                    requirement: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var requirementId: string = $stateParams['id'];
                        return dataAccessService.getRequirementResource().get({ id: requirementId }).$promise;
                    }
                }
            });
    }
}