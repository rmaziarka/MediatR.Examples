/// <reference path="../../typings/_all.d.ts" />

module Antares {

    export module Common {

        export module Factories {

            export class LocalizationLoaderFactory {

                constructor(private $q: ng.IQService, private dataAccessService: Antares.Services.DataAccessService) {
                }

                getTranslation = (options: any) => {

                    var deferred = this.$q.defer();

                    var staticTranslationsPromise = this.dataAccessService.getStaticTranslationResource().get({ isoCode: options.key }).$promise;
                    var enumTranslationsPromise = this.dataAccessService.getEnumTranslationResource().get({ isoCode: options.key }).$promise;

                    this.$q.all([staticTranslationsPromise, enumTranslationsPromise]).then(
                        (promisesResults: any) => {
                            var translations = promisesResults[0].toJSON();
                            var enums = promisesResults[1].toJSON();
                            translations.ENUMS = enums;
                            deferred.resolve(translations);
                        },
                        () => {
                            deferred.reject(options.key);
                        });

                    return deferred.promise;
                };

                static getInstance($q: ng.IQService, dataAccessService: Antares.Services.DataAccessService) {
                    return (options: any) => {
                        var factory = new LocalizationLoaderFactory($q, dataAccessService);
                        return factory.getTranslation(options);
                    };
                }
            }

            angular.module('app').factory('LocalizationLoaderFactory', LocalizationLoaderFactory.getInstance);
        }
    }
}