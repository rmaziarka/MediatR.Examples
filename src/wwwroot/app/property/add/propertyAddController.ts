/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    export class PropertyAddController {
        public save() {
            alert("Saved");
        }      
    }

    angular.module('app').controller('PropertyAddController', PropertyAddController);
}