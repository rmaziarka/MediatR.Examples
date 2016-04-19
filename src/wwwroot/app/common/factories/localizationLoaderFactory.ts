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
                    var resourceTranslationsPromise = this.dataAccessService.getResourceTranslationResource().get({ isoCode: options.key }).$promise;
                    
                    this.$q.all([staticTranslationsPromise, enumTranslationsPromise, resourceTranslationsPromise]).then(
                        (promisesResults: any) => {
                            var translations = promisesResults[0].toJSON();
                            var enums = promisesResults[1].toJSON();
                            var resources = promisesResults[2].toJSON();

                            var dynamicTranslations: any = {};
                            angular.extend(dynamicTranslations, enums, resources);
                            
                            translations[Common.Filters.DynamicTranslateFilter.DYNAMIC_TRANSLATIONS_KEY] = dynamicTranslations;
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