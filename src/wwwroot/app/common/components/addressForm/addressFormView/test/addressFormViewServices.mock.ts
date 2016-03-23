/// <reference path="../../../../../typings/_all.d.ts" />
/// <reference path="../../../../../common/models/dto/AddressForm.ts" />
/// <reference path="../addressFormViewController.ts" />

module Antares.Mock.AddressForm {
    export var FullAddress = new Antares.Common.Models.Dto.Address("addr1", "country1", "a1", "Castle", "1", "Line 1", "Line 2", "Line 3", "W11 997", "London", "GB");
    export var AddressFormWithOneLine =
        new Antares.Common.Models.Dto.AddressForm('a1', 'GB', [new Antares.Common.Models.Dto.AddressFormFieldDefinition("City", "TownLabelKey", true, "[a-zA-Z]", 0, 0, 1)]);

    export function mockHttpResponce($httpBackend: ng.IHttpBackendService, addressFormId: string = '', respond: any): ng.IHttpBackendService {

        $httpBackend.whenGET(new RegExp(`\/api\/addressForm\/${addressFormId}`)).respond(() => {
            return respond;
        });

        return $httpBackend;
    }
}
