/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import IOwnership = Antares.Common.Models.Dto.IOwnership;
    import IContact = Antares.Common.Models.Dto.IContact;

    export class OwnershipAddController {
        static $inject = ['componentRegistry', 'dataAccessService'];
        
        componentId: string;
        ownership: IOwnership = <IOwnership>{};

        constructor(
            componentRegistry: Antares.Core.Service.ComponentRegistry,
            private dataAccessService: Antares.Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);
        }

        loadOwnership = (contacts: IContact[]) =>{
            this.ownership.contacts = contacts;
        }

        getOwnership = () =>{
            return this.ownership;
        }
    }

    angular.module('app').controller('OwnershipAddController', OwnershipAddController);
}