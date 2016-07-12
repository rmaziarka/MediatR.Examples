/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;
    import Commands = Common.Models.Commands;

    export class TenancyService {
        private url: string = '/api/tenancies/';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        getTenancy = (id: string): ng.IHttpPromise<Dto.ITenancy> => {
            return this.$http.get(this.appConfig.rootUrl + this.url + id)
                .then<Dto.ITenancy>((result: ng.IHttpPromiseCallbackArg<Dto.ITenancy>) => result.data);
        }
    }

    angular.module('app').service('tenancyService', TenancyService);
};