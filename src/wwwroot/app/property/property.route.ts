/// <reference path="../typings/_all.d.ts" />

module Antares.Property {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider) {
        $stateProvider
            .state('app.property-view', {
                url: '/property/view',
                templateUrl: 'app/property/view/propertyView.html',
                controllerAs: 'vm',
                controller: 'propertyViewController'
            });
    }
}