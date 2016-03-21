/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import CountryLocalised = Common.Models.Dto.CountryLocalised;
    import Address = Common.Models.Dto.Address;
    import AddressForm = Common.Models.Dto.AddressForm;
    import AddressFormFieldDefinition = Antares.Common.Models.Dto.AddressFormFieldDefinition;

    export class AddressFormEditController {
        public entityTypeCode: string;
        public address: Address;

        private countries: CountryLocalised[] = [];
        private addressForm: AddressForm;

        private countryResource: Common.Models.Resources.ICountryResourceClass;
        private addressFormResource: Common.Models.Resources.IAddressFormResourceClass;

        constructor(private dataAccessService: Services.DataAccessService) {
            this.countryResource = dataAccessService.getCountryResource();
            this.addressFormResource = dataAccessService.getAddressFormResource();

            this.countryResource
                .query({ entityTypeCode : this.entityTypeCode })
                .$promise
                .then((data: any) =>{
                    this.countries = data;

                    if (this.address.countryId) {
                        this.getAddressFormTemplete(this.entityTypeCode, this.address.countryId);
                    }
                });
        }

        public changeCountry = (countryId: string): void =>{
            this.address.clear();

            this.getAddressFormTemplete(this.entityTypeCode, countryId);
        }

        public isSubmitted = (form) => {
            while (!!form) {
                if (form.$submitted) return true;
                form = form.$$parentForm;
            }
            return false;
        };

        private getAddressFormTemplete = (entityTypeCode: string, countryId: string): void =>{
            var countryLocalised: CountryLocalised = _.find(this.countries, (c) =>{
                 return c.country.id === countryId;
            });

            this.addressFormResource
                .get({ entityTypeCode: entityTypeCode, countryCode: countryLocalised.country.isoCode })
                .$promise
                .then((data: any) =>{
                    var id: string = data.id,
                        countryId: string = data.countryId,
                        addressFieldDefinitions: AddressFormFieldDefinition[] = data.addressFieldDefinitions;

                    this.addressForm = new AddressForm(id, countryId, addressFieldDefinitions);
                    this.address.addressFormId = this.addressForm.id;
                });
        };
    }

    angular.module('app').controller('AddressFormEditController', AddressFormEditController);
}