/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class AddressFormEditController {
        public userCountryCode: string;
        public entityTypeCode: string;
        public address: Business.Address;

        public isLoading: boolean = true;
        public countries: Business.CountryLocalised[] = [];
        public addressForm: Business.AddressForm;

        private countryResource: Common.Models.Resources.ICountryResourceClass;
        private addressFormResource: Common.Models.Resources.IAddressFormResourceClass;

        constructor(private dataAccessService: Services.DataAccessService) {
            this.countryResource = dataAccessService.getCountryResource();
            this.addressFormResource = dataAccessService.getAddressFormResource();

            this.countryResource
                .query({ entityTypeCode: this.entityTypeCode })
                .$promise
                .then((data: any) => {
                    this.countries = data;

                    var isNewAddressMode = !this.address.countryId;
                    if (isNewAddressMode) {
                        this.setDefaultCountry();
                    }

                    this.getAddressFormTemplete(this.entityTypeCode, this.address.countryId);
                })
                .finally(() => {
                    this.isLoading = false;
                });
        }

        public changeCountry = (countryId: string): void => {
            this.address.clear();
            this.address.countryId = countryId;

            this.getAddressFormTemplete(this.entityTypeCode, countryId);
        }

        public isSubmitted = (form: any) => {
            while (!!form) {
                if (form.$submitted) return true;
                form = form.$$parentForm;
            }
            return false;
        };

        private setDefaultCountry = (): void => {
            var userCountry: Business.CountryLocalised = _.find(this.countries, (c: Business.CountryLocalised) => {
                return c.country.isoCode === this.userCountryCode;
            });

            this.address.countryId = userCountry && userCountry.country ? userCountry.country.id : null;
        }

        private getAddressFormTemplete = (entityTypeCode: string, countryId: string): void => {
            var countryLocalised: Business.CountryLocalised = _.find(this.countries, (c: Business.CountryLocalised) => {
                return c.country.id === countryId;
            });

            this.addressFormResource
                .get({ entityTypeCode: entityTypeCode, countryCode: countryLocalised.country.isoCode })
                .$promise
                .then((data: Dto.IAddressForm) => {
                    this.addressForm = new Business.AddressForm(data.id, data.countryId, data.addressFieldDefinitions);
                    this.address.addressFormId = this.addressForm.id;
                });
        };
    }

    angular.module('app').controller('AddressFormEditController', AddressFormEditController);
}