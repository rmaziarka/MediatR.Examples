/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;

    export class ActivityService {
        private url: string = '/api/activities/';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        addActivity = (activityCommand: Commands.ActivityAddCommand): ng.IHttpPromise<Dto.IActivity> => {
            return this.$http.post(this.appConfig.rootUrl + this.url, activityCommand)
                .then<Dto.IActivity>((result: ng.IHttpPromiseCallbackArg<Dto.IActivity>) => result.data);
        }

        updateActivity = (activityCommand: Commands.ActivityEditCommand): ng.IHttpPromise<Dto.IActivity> => {
            return this.$http.put(this.appConfig.rootUrl + this.url, activityCommand)
                .then<Dto.IActivity>((result: ng.IHttpPromiseCallbackArg<Dto.IActivity>) => result.data);
        }
    }

    angular.module('app').service('activityService', ActivityService);
};