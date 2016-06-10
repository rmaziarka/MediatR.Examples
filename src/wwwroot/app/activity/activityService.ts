/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityService {
        private url: string = '/api/activities/';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        addActivityPanel = (activity: AddPanel.ActivityAddPanelCommand): ng.IHttpPromise<Dto.IActivity> => {
            return this.$http.post(this.appConfig.rootUrl + this.url, activity);
        }
    }

    angular.module('app').service('activityService', ActivityService);
};