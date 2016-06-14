/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;

    export class UserService {

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig) {
        }

        getUserData(): ng.IPromise<Dto.IUserData> {
            return this.$http.get<Dto.IUserData>(this.appConfig.rootUrl + "/api/users/current")
                .then<Dto.IUserData>((result: ng.IHttpPromiseCallbackArg<Dto.IUserData>) => result.data);
        }
    }

    angular.module('app').service('userService', UserService);
}