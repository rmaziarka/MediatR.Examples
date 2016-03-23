/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../../../../common/models/dto/countryLocalised.ts" />
/// <reference path="../../../../common/models/dto/address.ts" />
/// <reference path="../../../../common/models/dto/addressForm.ts" />
/// <reference path="../../../../common/models/resources.d.ts" />
/// <reference path="../../../../common/services/dataaccessservice.ts" />

module Antares.Common.Component {
    import Address = Common.Models.Dto.Address;
    import CountryLocalised = Common.Models.Dto.CountryLocalised;
    import AddressForm = Common.Models.Dto.AddressForm;
    import IAddressForm = Common.Models.Dto.IAddressForm;

    export class AddressFormEditController {
        public entityTypeCode: string;
        public address: Address;

        public isLoading: boolean = true;
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

                    if (this.address && this.address.countryId) {
                        this.getAddressFormTemplete(this.entityTypeCode, this.address.countryId);
                    }
                })
                .finally(() => {
                    this.isLoading = false;
                });
        }

        public changeCountry = (countryId: string): void =>{
            this.address.clear();
            this.address.countryId = countryId;

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
                .then((data: IAddressForm) =>{
                    this.addressForm = new AddressForm(data.id, data.countryId, data.addressFieldDefinitions);
                    this.address.addressFormId = this.addressForm.id;
                });
        };
    }

    angular.module('app').controller('AddressFormEditController', AddressFormEditController);
}