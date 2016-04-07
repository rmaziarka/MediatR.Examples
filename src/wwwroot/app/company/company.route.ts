/// <reference path="../typings/_all.d.ts" />

module Antares.Company {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.company-add', {
                url: '/company/add',
                templateUrl: 'app/company/add/companyAdd.html',
                controllerAs: 'vm',
                controller: 'CompanyAddController'
            });
    }
}