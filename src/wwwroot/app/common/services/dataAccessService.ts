/// <reference path="../../typings/_all.d.ts" />
declare var KnightFrankAntaresConfig;

module Antares.Services {
    import Resources = Common.Models.Resources;

    export class DataAccessService {
        constructor(private $resource: ng.resource.IResourceService) {
        }

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            //TODO move url to config
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource(KnightFrankAntaresConfig.api.rootUrl + '/api/contacts/:id');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}