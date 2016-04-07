/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class AddressForm implements Dto.IAddressForm {
        public addressFieldRows: AddressFormFieldDefinition[][];

        constructor(
            public id: string = "",
            public countryId: string = "",
            public addressFieldDefinitions: AddressFormFieldDefinition[] = []
        ) {
            this.updateAddressFieldRows();
        }

        updateAddressFieldRows() {
            _.forEach(this.addressFieldDefinitions, (field: AddressFormFieldDefinition) => {
                field.name = field.name.charAt(0).toLowerCase() + field.name.slice(1);
            });

            this.addressFieldRows = _.toArray<AddressFormFieldDefinition[]>(_.groupBy<AddressFormFieldDefinition>(this.addressFieldDefinitions, 'rowOrder'));
        }
    }
}