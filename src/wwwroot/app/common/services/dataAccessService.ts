/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Resources = Common.Models.Resources;

    export class DataAccessService {

        private rootUrl: string = "";

        constructor(private $resource: ng.resource.IResourceService, private appConfig: Antares.Common.Models.IAppConfig) {

        }

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource(this.appConfig.rootUrl + '/api/contact/:id');
        }

        getRequirementResource(): Resources.IBaseResourceClass<Resources.IRequirementResource> {
            return <Resources.IBaseResourceClass<Resources.IRequirementResource>>
                this.$resource(this.appConfig.rootUrl + '/api/requirement/:id');
        }

        getOwnershipResource(): Resources.IBaseResourceClass<Resources.IOwnershipResource> {
            return <Resources.IBaseResourceClass<Resources.IOwnershipResource>>
                this.$resource(this.appConfig.rootUrl + '/api/ownership/:id');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}