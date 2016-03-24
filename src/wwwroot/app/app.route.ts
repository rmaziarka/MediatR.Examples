/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: any, $urlRouterProvider: any) {
        $stateProvider
            .state('app', {
                url: '/app',
                abstract: true,
                templateUrl: 'app/app.html',
                controller: 'appController',
                controllerAs: 'appVm',
                resolve: {
                    'userData': (userService: Antares.Services.UserService) => {
                        return userService.getUserData();
                    }
                }
            });

        $stateProvider
            .state('app.contact-add', {
                url: '/contact/add',
                params: {},
                template: '<contact-add></contact-add>'
            });

        $urlRouterProvider.otherwise('/app/contact/add');
    }
}