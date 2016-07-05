/// <reference path="../../typings/_all.d.ts" />


module Antares.Services {
    import IActivityConfig = Activity.IActivityConfig;
    import Attributes = Antares.Attributes;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;
    import IRequirementConfig = Requirement.IRequirementConfig;
    import IOfferConfig = Offer.IOfferConfig;

    export class ConfigService {

        private apiUrl: string = '/api/metadata';

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig, private $q: ng.IQService) {
        }

        public getActivity = (pageType: PageTypeEnum, propertyTypeId: string, activityTypeId: string, entity: any): ng.IHttpPromise<IActivityConfig> =>{
            var routeUrl = '/attributes/activity';
            var postUrl = this.appConfig.rootUrl + this.apiUrl + routeUrl;

            var params = {
                pageType: pageType,
                propertyTypeId: propertyTypeId || '00000000-0000-0000-0000-000000000000',
                activityTypeId: activityTypeId || '00000000-0000-0000-0000-000000000000'
            }

            return this.$http
                .post<IActivityConfig>(postUrl, entity, { params: params})
                .then<IActivityConfig>((result: ng.IHttpPromiseCallbackArg<IActivityConfig>) => result.data);
        }

        public getRequirement = (pageType: PageTypeEnum, requirementTypeId: string, entity: any): ng.IHttpPromise<IRequirementConfig> => {
            var routeUrl = '/attributes/requirement';
            var postUrl = this.appConfig.rootUrl + this.apiUrl + routeUrl;

            var params = {
                pageType: pageType,
                requirementTypeId: requirementTypeId || '00000000-0000-0000-0000-000000000000'
            }

            return this.$http
                .post<IRequirementConfig>(postUrl, entity, { params: params })
                .then<IRequirementConfig>((result: ng.IHttpPromiseCallbackArg<IRequirementConfig>) => result.data);
        }

        public getOffer = (pageType: PageTypeEnum, requirementTypeId: string, offerTypeId: string, entity: any): ng.IHttpPromise<IOfferConfig> =>{
            var routeUrl = '/attributes/offer';
            var postUrl = this.appConfig.rootUrl + this.apiUrl + routeUrl;

            var params = {
                pageType: pageType,
                requirementTypeId: requirementTypeId || '00000000-0000-0000-0000-000000000000',
                offerTypeId: offerTypeId || '00000000-0000-0000-0000-000000000000'
            }

            return this.$http
                .post<IOfferConfig>(postUrl, entity, { params: params })
                .then<IOfferConfig>((result: ng.IHttpPromiseCallbackArg<IOfferConfig>) => result.data);
        }
    }

    angular.module('app').service('configService', ConfigService);
}