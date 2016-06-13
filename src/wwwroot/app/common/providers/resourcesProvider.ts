/// <reference path="../../typings/_all.d.ts" />


module Antares.Providers {
    import IActivityType = Antares.Common.Models.Dto.IActivityType;
    import ResourcesService = Antares.Services.ResourcesService;

    export class ResourcesProvider {

        public activityTypes: IActivityType[];

        constructor(private resourcesService: ResourcesService, private $q: ng.IQService) {
        }

        public load = (): ng.IPromise<any> =>{
            return this.$q.all([this.loadActivityTypes()]);
        }

        private loadActivityTypes = (): ng.IPromise<any> => {
            return this.resourcesService.getActivityTypes()
                .then((activityTypes : IActivityType[]) => {
                    this.activityTypes = activityTypes;
                });
        }
    }

    angular.module('app').service('resourcesProvider', ResourcesProvider);
}