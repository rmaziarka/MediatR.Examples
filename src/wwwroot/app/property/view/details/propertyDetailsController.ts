/// <reference path="../../../typings/_all.d.ts" />
/// <reference path="../../../common/models/resources.d.ts" />

module Antares.Property.View.Details {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class PropertyDetailsController {
        property: Business.Property;
        userData: Dto.IUserData;
    }

    angular.module('app').controller('propertyDetailsController', PropertyDetailsController);
}