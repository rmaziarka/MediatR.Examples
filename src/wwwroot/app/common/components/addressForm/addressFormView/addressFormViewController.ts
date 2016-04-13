/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    'use strict';

    import Business = Common.Models.Business;

    export class AddressFormViewController {
        addressForm: any;
        address: Business.Address;

        constructor(private dataAccessService: Services.DataAccessService, $scope: ng.IScope) {
            this.dataAccessService.getAddressFormResource().get({ id: this.address.addressFormId }).$promise.then((result)=>{
                var addressForm = new Business.AddressForm();
                angular.extend(addressForm, result);
                addressForm.updateAddressFieldRows();
                this.addressForm = addressForm;
            });
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