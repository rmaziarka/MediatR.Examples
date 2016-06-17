/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;

    export class UserService {

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig) {
        }

        getCurrentUser(): ng.IPromise<Dto.ICurrentUser> {
            return this.$http.get<Dto.ICurrentUser>(this.appConfig.rootUrl + "/api/users/current")
                .then<Dto.ICurrentUser>((result: ng.IHttpPromiseCallbackArg<Dto.ICurrentUser>) => result.data);
        }
    }

    angular.module('app').service('userService', UserService);
}