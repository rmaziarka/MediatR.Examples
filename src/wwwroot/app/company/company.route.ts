/// <reference path="../typings/_all.d.ts" />

module Antares.Company {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider){
        $stateProvider
            .state('app.company-add', {
                url : '/company/add',
                params : {},
                template : '<company-add></company-add>'
            })
            .state('app.company-view', {
                url : '/company/:id',
                template : '<company-view company="company"></company-view>',
                controller: "CompanyRouteController",
                resolve : {
                    company : ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) =>{
                        var companyId: string = $stateParams['id'];
                        return dataAccessService.getCompanyResource().get({ id : companyId }).$promise;
                    }
                }
            })
            .state('app.company-edit', {
                url : '/company/edit/:id',
                params : {},
                template: '<company-edit company="company"></company-edit>',
                controller: "CompanyRouteController",
                resolve: {
                    company: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var companyId: string = $stateParams['id'];
                        return dataAccessService.getCompanyResource().get({ id: companyId }).$promise;
                    }
                }
            });
    }
}