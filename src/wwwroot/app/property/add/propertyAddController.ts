/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Antares.Common.Models.Dto;

    export class PropertyAddController {
        public entityTypeCode: string = 'Property';
        public property: Dto.Property = new Dto.Property();

        public save() {
            alert("Saved: " + JSON.stringify(this.property.address));
        }
    }

    angular.module('app').controller('PropertyAddController', PropertyAddController);
}