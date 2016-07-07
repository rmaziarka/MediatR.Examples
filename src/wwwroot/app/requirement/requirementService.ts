/// <reference path="../typings/_all.d.ts" />

module Antares.Requirement {
    import Dto = Common.Models.Dto;

    export class RequirementService {
        private apiUrl: string = '/api/requirements';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig){
        }

        createRequirementAttachment = (requirementAttachment: Requirement.Command.RequirementAttachmentSaveCommand): ng.IHttpPromise<Dto.IAttachment> =>{
            var url = `${this.apiUrl}/${requirementAttachment.requirementId}/attachments/`;

            return this.$http.post(this.appConfig.rootUrl + url, requirementAttachment);
        }

        getRequirementTypes = (countryCode: string): ng.IHttpPromise<Array<Dto.IRequirementTypeQueryResult>> => {
            var url = `${this.apiUrl}/types?countryCode=${countryCode}`;

            return this.$http.get(this.appConfig.rootUrl + url);
        }
    }

    angular.module('app').service('requirementService', RequirementService);
};