/// <reference path="../../../../../typings/_all.d.ts" />

module Antares.Mock.AddressForm {
    export var FullAddress = new Common.Models.Dto.Address("addr1", "country1", "a1", "Castle", "1", "Line 1", "Line 2", "Line 3", "W11 997", "London", "GB");
    export var AddressFormWithOneLine =
        new Common.Models.Dto.AddressForm('a1', 'GB', [
            new Common.Models.Dto.AddressFormFieldDefinition("City", "TownLabelKey", true, "[a-zA-Z]", 2, 0, 1),
            new Common.Models.Dto.AddressFormFieldDefinition("Line3", "Line3LabelKey", true, "[a-zA-Z]", 1, 0, 1)
            ]);

    export function mockHttpResponce($httpBackend: ng.IHttpBackendService, addressFormId: string = '', respond: any): ng.IHttpBackendService {

        $httpBackend.whenGET(new RegExp(`\/api\/addressForm\/${addressFormId}`)).respond(() => {
            return respond;
        });

        return $httpBackend;
    }
}
