/// <reference path="../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyService {
        private apiUrl: string = '/api/properties';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        createPropertyAttachment = (propertyAttachment: Antares.Property.Command.PropertyAttachmentSaveCommand): ng.IHttpPromise<Dto.IAttachment> => {
            var url = `${this.apiUrl}/${propertyAttachment.propertyId}/attachments/`;

            return this.$http.post(this.appConfig.rootUrl + url, propertyAttachment);
        }
    }

    angular.module('app').service('propertyService', PropertyService);
};