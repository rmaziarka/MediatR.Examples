/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Resources = Antares.Common.Models.Resources;
    
    export class ConfigService {
        public promise: ng.IPromise<string>;

        constructor(private $http: ng.IHttpService) {
            this.promise = this.getRootUrl();
        }

        public getRootUrl = (): ng.IPromise<string> =>
        {
            return this.$http.get('app.json').then(function (data: any) {
                return data.data["App.Settings"].Api.RootUrl;
            });
        }
    }

    angular.module('app').service('configService', ConfigService);
}