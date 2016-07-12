    /// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import IContactTitle = Common.Models.Dto.IContactTitle;

    export class ContactTitleService {
        private url: string = '/api/contacts/titles';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        get = (): ng.IHttpPromise<IContactTitle[]> => {
            return this.$http
                .get<IContactTitle[]>(this.appConfig.rootUrl + this.url);
    }
}

    angular.module('app').service('contactTitleService', ContactTitleService);
};