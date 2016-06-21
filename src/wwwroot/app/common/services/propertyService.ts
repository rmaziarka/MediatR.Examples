/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyService {

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig) {
        }

        getSearchResult(query: string, page: number, size: number) {
            var getParams: ng.IRequestShortcutConfig = { params: { query: query, page: page, size: size } };

            return this.$http.get<Dto.IPropertySearchResult>(this.appConfig.rootUrl + "/api/properties/search", getParams)
                .then<Dto.IPropertySearchResult>((result: ng.IHttpPromiseCallbackArg<Dto.IPropertySearchResult>) => new Business.PropertySearchResult(<Dto.IPropertySearchResult>result.data));
        }
    }

    angular.module('app').service('propertyService', PropertyService);
}