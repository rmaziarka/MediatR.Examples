/// <reference path="../../typings/_all.d.ts" />

module Antares.Property.Preview {
    import Business = Common.Models.Business;

    export class PropertyPreviewCardController {
        // binding
        property: Business.PreviewProperty;
    }

    angular.module('app').controller('PropertyPreviewCardController', PropertyPreviewCardController);
}