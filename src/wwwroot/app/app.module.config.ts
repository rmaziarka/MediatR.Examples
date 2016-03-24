/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(['$translateProvider', initTranslations]);
    app.config(['$provide', extendOrderByWithEmptyFields]);

    function initTranslations($translateProvider: any) {
        $translateProvider
            .useStaticFilesLoader({
                prefix: 'translations/',
                suffix: '.json'
            })
            .registerAvailableLanguageKeys(['en'], {
                'en_*': 'en'
            })
            .preferredLanguage('en')
            .useSanitizeValueStrategy('escape');
    }

    function extendOrderByWithEmptyFields($provider: angular.auto.IProvideService){
        $provider.decorator('orderByFilter', Antares.Common.Decorators.OrderByFilterDecorator.decoratorFunction);
    }
}