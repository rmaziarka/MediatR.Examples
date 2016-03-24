/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    'use strict';

    import AddressForm = Antares.Common.Models.Dto.AddressForm;
    import AddressFormFieldDefinition = Antares.Common.Models.Dto.AddressFormFieldDefinition;
    import Address = Antares.Common.Models.Dto.Address;
    import AddressFormResource = Antares.Common.Models.Resources.IAddressFormResource;

    export class AddressFormViewController {
        addressForm: any;
        address: Address;

        constructor(private dataAccessService: Services.DataAccessService, $scope: ng.IScope) {
            this.dataAccessService.getAddressFormResource().get({ id: this.address.addressFormId }).$promise.then((result)=>{
                var addressForm = new AddressForm();
                angular.extend(addressForm, result);
                addressForm.updateAddressFieldRows();
                this.addressForm = addressForm;
            });
        }
    }

    angular
        .module('app')
        .controller('addressFormViewController', AddressFormViewController);
}