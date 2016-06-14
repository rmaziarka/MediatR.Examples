/// <reference path="../../typings/_all.d.ts" />


module Antares.Services {
    import IActivityConfig = Antares.Activity.IActivityConfig;
    import IActivityAddPanelConfig = Antares.Activity.IActivityAddPanelConfig;
    import Attributes = Antares.Attributes;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;

    export class ConfigService {

        private apiUrl: string = '/api/metadata';

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig, private $q: ng.IQService) {
        }

        public getActivity = (pageType: PageTypeEnum, propertyTypeId: string, activityTypeId: string, entity: any): ng.IHttpPromise<IActivityConfig> =>{
            var routeUrl = '/attributes/activity';
            var postUrl = this.appConfig.rootUrl + this.apiUrl + routeUrl;

            var params = {
                pageType: pageType,
                propertyTypeId: propertyTypeId,
                activityTypeId: activityTypeId
            }

            return this.$http
                .post<IActivityConfig>(postUrl, entity, { params: params})
                .then<IActivityConfig>((result: ng.IHttpPromiseCallbackArg<IActivityConfig>) => result.data);
        }
    }

    angular.module('app').service('configService', ConfigService);
}