/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Address = Common.Models.Dto.Address;
    import IAddress = Antares.Common.Models.Dto.IAddress;

    export class PropertyAddController {
        public entityTypeCode: string = 'Property';

        public address: IAddress = new Address();

        public save() {
            alert("Saved: " + JSON.stringify(this.address));
        }
    }

    angular.module('app').controller('PropertyAddController', PropertyAddController);
}