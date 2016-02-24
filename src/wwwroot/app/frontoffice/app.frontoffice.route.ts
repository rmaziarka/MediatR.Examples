/// <reference path="../typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app.frontoffice');

    app.config(initRoute);

    function initRoute($stateProvider) {
        $stateProvider
            .state('contact-add', {
                url: '/contact/add',
                params: {},
                templateUrl: 'app/frontoffice/contact/add/contactAdd.html',
                controllerAs: 'vm',
                controller: 'ContactAddController'
            });
    }
}