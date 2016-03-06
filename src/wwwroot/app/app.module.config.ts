/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(['$translateProvider', initTranslations]);

    function initTranslations($translateProvider) {
        $translateProvider
            .useStaticFilesLoader({
                prefix: 'common/translations/',
                suffix: '.json'
            })
            .registerAvailableLanguageKeys(['en'], {
                'en_*': 'en'
            })
            .preferredLanguage('en')
            .useSanitizeValueStrategy('escape');
    }
}