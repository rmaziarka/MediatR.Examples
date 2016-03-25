/// <reference path="../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {
        $stateProvider
            .state('app.property-view', {
                url: '/property/view/:id',
                params: {},
                template: "<property-view property='property'></property-view>",
                controller: ($scope: ng.IScope, property: Dto.Property) => {
                    $scope['property'] = property;
                },
                resolve: {
                    property: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var propertyId: string = $stateParams['id'];
                        return dataAccessService.getPropertyResource().get({ id: propertyId }).$promise;
                    }
                }
            })
            .state('app.property-edit', {
                url: '/property/edit/:id',
                params: {},
                template: '<property-edit property="property"></property-edit>',
                controller: ($scope: ng.IScope, property: Dto.Property) => {
                    $scope['property'] = property;
                },
                resolve: {
                    property: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var propertyId: string = $stateParams['id'];
                        return dataAccessService.getPropertyResource().get({ id: propertyId }).$promise;
                    }
                }
            })
            .state('app.property-add', {
                url: '/property/add',
                params: {},
                template: '<property-add user-data="appVm.userData"></property-add>'
            });

    }
}