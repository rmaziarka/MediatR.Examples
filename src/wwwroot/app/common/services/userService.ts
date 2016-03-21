/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import IUserData = Antares.Common.Models.Dto.IUserData;

    export class UserService {
        
        constructor(private $http: ng.IHttpService, private appConfig: Antares.Common.Models.IAppConfig) {

        }

        getUserData(): ng.IPromise<IUserData> {
            return this.$http.get<IUserData>(this.appConfig.rootUrl + "/api/user/data")
                .then<IUserData>((result: ng.IHttpPromiseCallbackArg<IUserData>) => result.data);
        }
    }

    angular.module('app').service('userService', UserService);
}