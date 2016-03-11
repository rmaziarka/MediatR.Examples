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
                this.$resource(this.rootUrl + '/api/contacts/:id');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}