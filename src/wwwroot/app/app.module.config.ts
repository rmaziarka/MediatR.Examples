/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(['$translateProvider', initTranslations]);
    app.run(configureRootUrl);    

    function initTranslations($translateProvider) {
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

    function configureRootUrl(configService: Antares.Services.ConfigService, dataAccessService: Antares.Services.DataAccessService)
    {
        configService.promise.then(function (data) {
            dataAccessService.setRootUrl(data);
        });
    }
}