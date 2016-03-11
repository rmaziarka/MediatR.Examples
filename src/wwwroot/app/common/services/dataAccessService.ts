/// <reference path="../../typings/_all.d.ts" />
///<reference path="../../../typings/main.d.ts"/>

module Antares.Services {
    import Resources = Antares.Common.Models.Resources;

    export class DataAccessService {

        private rootUrl: string = "";

        constructor(private $resource: ng.resource.IResourceService) {

        }

        setRootUrl(url: string): void {
            this.rootUrl = url;
        }

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource(this.rootUrl + '/api/contact/:id');
        }

        getRequirementResource(): Resources.IBaseResourceClass<Resources.IRequirementResource> {
            return <Resources.IBaseResourceClass<Resources.IRequirementResource>>
                this.$resource(this.rootUrl + '/api/requirement/:id');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}