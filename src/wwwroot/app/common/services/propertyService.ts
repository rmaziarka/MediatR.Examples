/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyService {
        private apiUrl: string = '/api/properties';

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig) {
        }

        getSearchResult(query: string, page: number, size: number) {
            var getParams: ng.IRequestShortcutConfig = { params: { query: query, page: page, size: size } };
            var url = `${this.apiUrl}/search`;

            return this.$http.get<Dto.IPropertySearchResult>(this.appConfig.rootUrl + url, getParams)
                .then<Dto.IPropertySearchResult>((result: ng.IHttpPromiseCallbackArg<Dto.IPropertySearchResult>) => new Business.PropertySearchResult(<Dto.IPropertySearchResult>result.data));
        }

        createPropertyAttachment = (propertyAttachment: Antares.Property.Command.PropertyAttachmentSaveCommand): ng.IHttpPromise<Dto.IAttachment> => {
            var url = `${this.apiUrl}/${propertyAttachment.propertyId}/attachments/`;

            return this.$http.post(this.appConfig.rootUrl + url, propertyAttachment);
        }

        getProperties = (): ng.IHttpPromise<Dto.IPreviewProperty[]> => {
            var url = `${this.apiUrl}`;

            return this.$http
                .get(this.appConfig.rootUrl + url)
                .then<Array<Dto.IPreviewProperty[]>>((result: ng.IHttpPromiseCallbackArg<Array<Dto.IPreviewProperty[]>>) => result.data);
        }
    }

    angular.module('app').service('propertyService', PropertyService);
}