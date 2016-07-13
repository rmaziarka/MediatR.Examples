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

        addTenancy = (tenancyCommand: Commands.Tenancy.TenancyAddCommand): ng.IHttpPromise<Dto.ITenancy> => {
            return this.$http.post(this.appConfig.rootUrl + this.url, tenancyCommand)
                .then<Dto.ITenancy>((result: ng.IHttpPromiseCallbackArg<Dto.ITenancy>) => result.data);
        }

        updateActivity = (tenancyCommand: Commands.Tenancy.TenancyEditCommand): ng.IHttpPromise<Dto.ITenancy> => {
            return this.$http.put(this.appConfig.rootUrl + this.url, tenancyCommand)
                .then<Dto.ITenancy>((result: ng.IHttpPromiseCallbackArg<Dto.ITenancy>) => result.data);
        }

        getTenancy = (id: string): ng.IHttpPromise<Dto.ITenancy> => {
            return this.$http.get(this.appConfig.rootUrl + this.url + id)
                .then<Dto.ITenancy>((result: ng.IHttpPromiseCallbackArg<Dto.ITenancy>) => result.data);
        }

        getTenancyTypes = (): ng.IHttpPromise<Dto.IResourceType[]> => {
            return this.$http.get(this.appConfig.rootUrl + this.url + 'types')
                .then<Dto.IResourceType[]>((result: ng.IHttpPromiseCallbackArg<Dto.IResourceType[]>) => result.data);
        }
    }

    angular.module('app').service('tenancyService', TenancyService);
};