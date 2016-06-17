/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    'use strict';

    import Business = Common.Models.Business;

    export class AddressFormViewController {
        addressForm: any;
        address: Business.Address;
        templateUrl: string;
        addressType: string;

        constructor(private addressFormsProvider: Providers.AddressFormsProvider) {
            var addressDefinitions = addressFormsProvider[this.addressType][this.address.countryId.toLowerCase()];

            var addressForm = new Business.AddressForm();
            angular.extend(addressForm, addressDefinitions);
            addressForm.updateAddressFieldRows();
            this.addressForm = addressForm;
        }
        
        isRowEmpty(row: Array<Business.AddressFormFieldDefinition>): boolean {
            return !_.any(row, (field: Business.AddressFormFieldDefinition) => {
                return !!this.address[field.name];
            });
        }
    }

    angular
        .module('app')
        .controller('addressFormViewController', AddressFormViewController);
}