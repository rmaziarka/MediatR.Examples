/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    export class OwnershipAddController {
        static $inject = ['componentRegistry', 'dataAccessService'];

        owners: any = [];
        componentId: string;

        constructor(
            componentRegistry: Antares.Core.Service.ComponentRegistry,
            private dataAccessService: Antares.Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);
        }

        loadOwnerships = (owners: any) =>{
            this.owners.push(owners);
        }

        public save() {
            alert("Saved");
        }      
    }

    angular.module('app').controller('OwnershipAddController', OwnershipAddController);
}