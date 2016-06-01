/// <reference path="../../typings/_all.d.ts" />


module Antares.Services {
    import ILatestViewResultItem = Common.Models.Dto.ILatestViewResultItem;
    import LatestViewCommand = Common.Models.Commands.ICreateLatestViewCommand;

    export class LatestViewsService {

        private apiUrl: string = '/api/latestviews';
        
        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig) {
        }

        public get = (): ng.IHttpPromise<ILatestViewResultItem[]> =>{
            return this.$http
                .get<ILatestViewResultItem[]>(this.appConfig.rootUrl + this.apiUrl);
        }

        public post = (command: LatestViewCommand): ng.IHttpPromise<ILatestViewResultItem[]> =>{
            return this.$http
                .post<ILatestViewResultItem[]>(this.appConfig.rootUrl + this.apiUrl, command);
        }
    }

    angular.module('app').service('latestViewsService', LatestViewsService);
}