/// <reference path="../../typings/_all.d.ts" />

module Antares.Property.View {
    export class PropertyViewController {
        static $inject = ['dataAccessService', 'componentRegistry', '$scope'];

    }
    angular.module('app').controller('propertyViewController', PropertyViewController);
}