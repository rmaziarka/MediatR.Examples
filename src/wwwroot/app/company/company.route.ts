/// <reference path="../typings/_all.d.ts" />

module Antares.Company {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider){
        $stateProvider
            .state('app.company-add',
            {
                url : '/company/add',
                params : {},
                template : '<company-add></company-add>'
            })
            .state('app.company-view',
            {
                url: '/company/',
                template: '<company-view></company-view>'
    });
    }
}