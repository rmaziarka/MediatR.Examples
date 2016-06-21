/// <reference path="../../../typings/_all.d.ts" />

module Antares.Mock {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class AddressFormsProviderMock {
        [index: string]: any;

        public property: Dto.IAddressFormList = {
            "country1": new Business.AddressForm('a1', 'GB', [
                new Business.AddressFormFieldDefinition("City", "TownLabelKey", true, "[a-zA-Z]", 2, 0, 1),
                new Business.AddressFormFieldDefinition("Line3", "Line3LabelKey", true, "[a-zA-Z]", 1, 0, 1)
            ])
        };

        public requirement: Dto.IAddressFormList = {
            "country1": new Business.AddressForm('a1', 'GB', [
                new Business.AddressFormFieldDefinition("City", "TownLabelKey", true, "[a-zA-Z]", 2, 0, 1),
                new Business.AddressFormFieldDefinition("Line3", "Line3LabelKey", true, "[a-zA-Z]", 1, 0, 1)
            ])
        };
    }
}