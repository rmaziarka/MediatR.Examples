/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;

    export class OwnershipViewController {
        static $inject = ['componentRegistry'];

        componentId: string;
        ownership: Dto.IOwnership = <Dto.IOwnership>{};

        constructor(
            componentRegistry: Core.Service.ComponentRegistry) {
            componentRegistry.register(this, this.componentId);
        }

        setOwnership = (ownership: Dto.IOwnership) => {
            this.ownership = ownership;
        }
    }

    angular.module('app').controller('OwnershipViewController', OwnershipViewController);
}