/// <reference path="../../typings/_all.d.ts" />

module Antares.Factories {
    import IUserData = Antares.Common.Models.Dto.IUserData;

    export class LocalizationLoaderFactory {

        constructor(private $http: ng.IHttpService, private $q: ng.IQService, private dataAccessService: Antares.Services.DataAccessService) { 
        }
        
        getTranslation = (options: any) => {

            var deferred = this.$q.defer();
            
            var staticTranslationsPromise = this.$http.get('/translations/' + options.key + '.json');            
            var enumTranslationsPromise = this.dataAccessService.getEnumItemTranslationResource().get({ isoCode: options.key }).$promise;

            this.$q.all([staticTranslationsPromise, enumTranslationsPromise]).then(
                (promisesResults : any) => {
                    var translations = promisesResults[0].data;
                    var enums = promisesResults[1].toJSON();
                    translations.ENUMS = enums;
                    deferred.resolve(translations);
                },
                () => {
                    deferred.reject(options.key);
                });

            return deferred.promise;
        };
    }

    function getInstance($http: ng.IHttpService, $q: ng.IQService, dataAccessService: Antares.Services.DataAccessService) {
        var factory = new LocalizationLoaderFactory($http, $q, dataAccessService);
        return (options:any) => {
            return factory.getTranslation(options);
        };
    }

    angular.module('app').factory('LocalizationLoaderFactory', getInstance);
}