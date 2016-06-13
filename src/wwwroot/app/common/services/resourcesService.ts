/// <reference path="../../typings/_all.d.ts" />


module Antares.Services {
    import IActivityType = Antares.Common.Models.Dto.IActivityType;

    export class ResourcesService {
        
        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig) {
        }

        public getActivityTypes = (): ng.IHttpPromise<IActivityType[]> =>{
            var apiUrl = '/api/activities';
            return this.$http
                .get<IActivityType[]>(this.appConfig.rootUrl + apiUrl)
                .then<IActivityType[]>((result: ng.IHttpPromiseCallbackArg<IActivityType[]>) => result.data);
        }
    }

    angular.module('app').service('resourcesService', ResourcesService);
}