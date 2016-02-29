/// <reference path="../../../typings/_all.d.ts" />

module Antares.Frontoffice.Services {
    import Resources = Common.Models.Resources;

    export class DataAccessService {
        constructor(private $resource: ng.resource.IResourceService) {
        }

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            //TODO move url to config
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource('/api/contact/:id');
        }
    }

    angular.module('app.frontoffice').service('dataAccessService', DataAccessService);
}