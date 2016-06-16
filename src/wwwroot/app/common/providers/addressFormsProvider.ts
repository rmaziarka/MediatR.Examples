/// <reference path="../../typings/_all.d.ts" />

module Antares.Providers {
    import Dto = Common.Models.Dto;

    export class AddressFormsProvider {
        constructor(private addressFormsService: Services.AddressFormsService) {
        }

        public propertyDefinitons: Dto.IAddressFormList;
        public requirementDefinitons: Dto.IAddressFormList;

        public loadDefinitions = () => {
            //this.addressFormsService.getAllDefinitons("Property")
            //    .then((result: Dto.IAddressFormList) => this.propertyDefinitons = result);

            //this.addressFormsService.getAllDefinitons("Requirement")
            //    .then((result: Dto.IAddressFormList) => this.requirementDefinitons = result);
        }
    }

    angular.module('app').service('addressFormsProvider', AddressFormsProvider);
}