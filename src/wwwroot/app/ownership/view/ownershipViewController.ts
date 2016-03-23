/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import IOwnership = Antares.Common.Models.Dto.IOwnership;

    export class OwnershipViewController {
        static $inject = ['componentRegistry'];
        
        componentId: string;
        ownership: IOwnership = <IOwnership>{};

        constructor(
            componentRegistry: Antares.Core.Service.ComponentRegistry) {
            componentRegistry.register(this, this.componentId);
        }

        setOwnership = (ownership) => {
            this.ownership = ownership;
        }
    }

    angular.module('app').controller('OwnershipViewController', OwnershipViewController);
}