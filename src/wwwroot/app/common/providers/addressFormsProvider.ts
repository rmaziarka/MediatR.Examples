/// <reference path="../../typings/_all.d.ts" />

module Antares.Providers {
    import Dto = Common.Models.Dto;

    export class AddressFormsProvider {
        [index: string]: any;

        public property: Dto.IAddressFormList;
        public requirement: Dto.IAddressFormList;

        constructor(private addressFormsService: Services.AddressFormsService, private $q: ng.IQService) {
        }

        public loadDefinitions = () => {
            return this.$q.all([
                this.addressFormsService.getAllDefinitons("Property")
                    .then((result: Dto.IAddressFormList) => this.property = result),
                this.addressFormsService.getAllDefinitons("Requirement")
                    .then((result: Dto.IAddressFormList) => this.requirement = result)
            ]);
        }
    }

    angular.module('app').service('addressFormsProvider', AddressFormsProvider);
}