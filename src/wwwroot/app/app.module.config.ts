/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(['$translateProvider', initTranslations]);
    app.config(['$provide', extendOrderByWithEmptyFields]);
    app.config(['$provide', extendNumberFilter]);
    app.config(['$provide', decorateInputNumber]);
    app.config(['$provide', decorateForm]);
    app.config(['growlProvider', configureGrowl]);
    app.config(['$httpProvider', setupInterceptors]);

    app.run(['enumService', initEnumService]);

    function initTranslations($translateProvider: any) {
        $translateProvider.useLoader('LocalizationLoaderFactory')
            .registerAvailableLanguageKeys(['en'], {
                'en_*': 'en'
            })
            .preferredLanguage('en');
    }

    function extendOrderByWithEmptyFields($provider: angular.auto.IProvideService) {
        $provider.decorator('orderByFilter', Common.Decorators.OrderByFilterDecorator.decoratorFunction);
    }

    function decorateInputNumber($provider: angular.auto.IProvideService) {
        $provider.decorator('inputDirective', Common.Decorators.InputNumberDirectiveDecorator.decoratorFunction);
    }

    function extendNumberFilter($provider: angular.auto.IProvideService){
        $provider.decorator('numberFilter', Common.Decorators.TrimZeroesNumberFilterDecorator.decoratorFunction);
    }

    function decorateForm($provider: angular.auto.IProvideService) {
        $provider.decorator('formDirective', Common.Decorators.FormDecorator.decoratorFunction);
    }

    function initEnumService(enumService: Services.EnumService) {
        enumService.init();
    }

    function configureGrowl(growlProvider: angular.growl.IGrowlProvider){
        growlProvider.globalTimeToLive({ success : 5000 });
        growlProvider.globalPosition('top-center');
        growlProvider.globalDisableCountDown(true);
    }

    function setupInterceptors($httpProvider: angular.IHttpProvider){
        $httpProvider.interceptors.push(Services.KfErrorInterceptor.factory);
    }
}