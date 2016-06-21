/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    'use strict';

    import Business = Common.Models.Business;

    export class AddressFormViewController {
        addressForm: Business.AddressForm;
        address: Business.Address;
        templateUrl: string;
        addressType: string;

        constructor(private addressFormsProvider: Providers.AddressFormsProvider) {
            var addressDefinitions = addressFormsProvider[this.addressType][this.address.countryId.toLowerCase()];

            var addressForm = new Business.AddressForm();
            angular.extend(addressForm, addressDefinitions);

            addressForm.normalizeFieldsName();
            addressForm.addressFieldDefinitions = this.pickAddressFieldDefinitions(addressForm.addressFieldDefinitions);
            addressForm.groupAddressFieldRows();

            this.addressForm = addressForm;
        }

        private pickAddressFieldDefinitions = (addressFieldDefinitions: Business.AddressFormFieldDefinition[]) => {
            var notEmptyMatchedDefinitions: Business.AddressFormFieldDefinition[] = [];

            _.each(addressFieldDefinitions, (fieldDefinition: Business.AddressFormFieldDefinition) => {
                if (this.address[fieldDefinition.name]) {
                    notEmptyMatchedDefinitions.push(fieldDefinition);
                }
            });

            return notEmptyMatchedDefinitions;
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