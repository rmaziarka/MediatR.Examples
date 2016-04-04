/// <reference path="../../../../../typings/_all.d.ts" />

module Antares.Mock.AddressForm {

    var address: Common.Models.Dto.IAddress = {
        id: 'addr1',
        countryId: 'country1',
        addressFormId: 'a1',
        propertyName: 'Castle',
        propertyNumber: '1',
        line1: 'Line 1',
        line2: 'Line 2',
        line3: 'Line 3',
        postcode: 'W11 997',
        city: 'London',
        county: 'GB'
    };

    export var FullAddress = new Common.Models.Dto.Address(address);
    export var AddressFormWithOneLine =
        new Common.Models.Dto.AddressForm('a1', 'GB', [
            new Common.Models.Dto.AddressFormFieldDefinition("City", "TownLabelKey", true, "[a-zA-Z]", 2, 0, 1),
            new Common.Models.Dto.AddressFormFieldDefinition("Line3", "Line3LabelKey", true, "[a-zA-Z]", 1, 0, 1)
            ]);

    export function mockHttpResponce($httpBackend: ng.IHttpBackendService, addressFormId: string = '', respond: any): ng.IHttpBackendService {

        $httpBackend.whenGET(new RegExp(`\/api\/addressForms\/${addressFormId}`)).respond(() => {
            return respond;
        });

        return $httpBackend;
    }
}
