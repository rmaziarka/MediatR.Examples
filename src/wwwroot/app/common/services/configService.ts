/// <reference path="../../typings/_all.d.ts" />


module Antares.Services {
    import IActivityConfig = Antares.Activity.IActivityConfig;
    import IActivityAddPanelConfig = Antares.Activity.IActivityAddPanelConfig;
    import Attributes = Antares.Attributes;

    export class ConfigService {

        private apiUrl: string = '/api/config';

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig, private $q: ng.IQService) {
        }

        public getActivity = (propertyTypeId: string, activityTypeId: string, activityStatusId: string): ng.IHttpPromise<IActivityConfig> =>{
            var defer = this.$q.defer();
            var result = <IActivityAddPanelConfig>{
                activityStatus: {
                    status: {
                        allowedCodes: ['PreAppraisal'], required: true, active: true
                    },
                    active: true
                },
                activityType: {
                    type: {
                        allowedCodes: ['Freehold Sale'], required: true, active: true
                    },
                    active: true
                }
            };

            if (activityTypeId) {
                result.vendors = { vendors : {required: true, active: true}, active: true };
            }

            defer.resolve(result);
            return defer.promise;


            var routeUrl = '/activity';

            return this.$http
                .get<IActivityConfig>(this.appConfig.rootUrl + this.apiUrl + routeUrl)
                .then<IActivityConfig>((result: ng.IHttpPromiseCallbackArg<IActivityConfig>) => result.data);
        }
    }

    angular.module('app').service('configService', ConfigService);
}