/// <reference path="../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {
        $stateProvider
            .state('app.property-search', {
                url: '/property/search',
                params: {},
                template: '<property-search></property-search>'
            })
            .state('app.property-add', {
                url: '/property/add',
                params: {},
                template: '<property-add user-data="appVm.userData"></property-add>'
            })
            .state('app.property-view', {
                url: '/property/:id',
                params: {},
                template: '<property-view user-data="appVm.userData" property="property"></property-view>',
                controller: 'PropertyRouteViewController',
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
                template: '<property-edit user-data="appVm.userData" property="property"></property-edit>',
                controller: 'PropertyRouteEditController',
                resolve: {
                    property: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var propertyId: string = $stateParams['id'];
                        return dataAccessService.getPropertyResource().get({ id: propertyId }).$promise;
                    }
                }
            });
    }
}