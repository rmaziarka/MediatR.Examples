/// <reference path="../../../../typings/_all.d.ts" />
module Antares {
    import Business = Antares.Common.Models.Business;

    describe('Givent requirement address view form', () => {
        var element: ng.IAugmentedJQuery,
            scope: ng.IScope,
            mockData = Antares.Mock.AddressForm.AddressFormWithOneLine,
            address = Antares.Mock.AddressForm.FullAddress;

        describe('and address form multiple field definition ', () => {
            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService, $httpBackend: ng.IHttpBackendService) => {

                Antares.Mock.AddressForm.mockHttpResponce($httpBackend, 'a1', [200, mockData]);

                scope = $rootScope.$new();
                scope['address'] = address;
                element = $compile('<requirement-address-form-view address="address"></requirement-address-form-view>')(scope);
                $httpBackend.flush();
                scope.$apply();
            }));

            it('when display address each item need to be in separate lines', () => {
                var row: number = element.find('div.row').length;
                expect(mockData.addressFieldRows.length).toBe(row);
            });

            it('should display address items in proper order',()=>{
               var firstItemLabel:string = element.find('div.row').first().find('div.addr-label span').text();
               var firstItemValue:string = element.find('div.row').first().find('div.addr-value span').text();

               var secondLine: Business.AddressFormFieldDefinition = mockData.addressFieldDefinitions[1];
               expect(firstItemLabel).toBe(secondLine.labelKey);
               expect(firstItemValue).toBe(address.line3);
            });
        })

    });
}