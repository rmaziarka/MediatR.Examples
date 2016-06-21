/// <reference path="typings/_all.d.ts" />

module Antares {
    import LatestViewsProvider = Providers.LatestViewsProvider;
    var app: ng.IModule = angular.module('app');

    app.config(['$stateProvider', '$urlRouterProvider', initRoute]);
    app.run(['$rootScope', 'growlMessages', onStart]);

    function initRoute($stateProvider: any, $urlRouterProvider: any) {
        $stateProvider
            .state('app', {
                url: '/app',
                abstract: true,
                templateUrl: 'app/app.html',
                controller: 'appController',
                controllerAs: 'appVm',
                resolve: {
                    'userData': (userService: Services.UserService) => {
                        return userService.getUserData();
                    },
                    'lastEntriesPromise': (latestViewsProvider: LatestViewsProvider) => {
                        return latestViewsProvider.refresh();
                    },
                    'enumsPromise': (enumService: Services.EnumService) => {
                        return enumService.getEnumPromise();
                    },
                    'enumsProviderPromise': (enumProvider: Providers.EnumProvider) =>{
                        return enumProvider.init();
                    },
                    'addressDefinitions': (addressFormsProvider: Providers.AddressFormsProvider) => {
                        return addressFormsProvider.loadDefinitions();
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

    function onStart($rootScope: ng.IRootScopeService, growlMessages: angular.growl.IGrowlMessagesService) {
        $rootScope.$on('$stateChangeSuccess', () => {
            growlMessages.destroyAllMessages();
        });
    }
}