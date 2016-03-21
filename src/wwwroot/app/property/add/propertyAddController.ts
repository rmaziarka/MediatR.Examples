/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    export class PropertyAddController {
        public entityTypeCode: string = 'Property';

        public address: any = { City: 'przekazane miasto', countryId: 'be8e1c1b-5fef-e511-828e-80c16efdf78c' };

        public save() {
            alert("Saved: " + JSON.stringify(this.address));
        }
    }

    angular.module('app').controller('PropertyAddController', PropertyAddController);
}