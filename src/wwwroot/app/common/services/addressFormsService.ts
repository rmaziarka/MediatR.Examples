/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;

    export class AddressFormsService {

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig) {
        }

        getAllDefinitons(entityType: string) {
            //var getParams: ng.IRequestShortcutConfig = { params: { entityType: entityType } };

            //return this.$http.get<Dto.IAddressFormList>(this.appConfig.rootUrl + "/api/addressforms/list", getParams)
            //    .then<Dto.IAddressFormList>((result: ng.IHttpPromiseCallbackArg<Dto.IAddressFormList>) => result.data);
        }
    }

    angular.module('app').service('addressFormsService', AddressFormsService);
}