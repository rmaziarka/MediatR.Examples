/// <reference path="../../../typings/_all.d.ts" />
/// <reference path="../../../common/models/dto/property.ts" />
/// <reference path="../../../common/models/dto/address.ts" />
/// <reference path="../../../common/models/dto/addressform.ts" />

module Antares {
    import Dto = Antares.Common.Models.Dto;
    import PropertyAddController = Antares.Property.PropertyAddController;

    describe('Given add property page is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            controller: PropertyAddController;

        var countriesMock = [{ country: { id: "countryId1", isoCode: "GB" }, locale: {}, value: "United Kingdom" }];

        describe('when page is loaded', () =>{
            var countryMockId = countriesMock[0].country.id,
                newPropertyMock: Dto.Property = new Dto.Property();

            newPropertyMock.id = 'propId1';
            newPropertyMock.address = new Dto.Address();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) =>{

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                $http.whenGET(/\/api\/resources\/countries\/addressform\?entityTypeItemCode=Property/).respond(() => {
                    return [200, countriesMock];
                });

                // compile
                element = compile('<property-add></property-add>')(scope);
                scope.$apply();
                controller = element.controller('propertyAdd');

                controller.property = newPropertyMock;
            }));

           it('then page displays address form component', () => {
                var addressFormComponent = element.find('address-form-edit');
                expect(addressFormComponent.length).toBe(1);
            });

           it('then save button is disabled if country is not selected', () => {
               newPropertyMock.address.countryId = '';
               scope.$apply();

               var button = element.find('button#saveBtn');
               expect(button[0].getAttribute('disabled')).toBeTruthy();
           });

           it('then save button is enabled if country is selected', () => {
               newPropertyMock.address.countryId = countryMockId;
               scope.$apply();

               var button = element.find('button#saveBtn');
               expect(button[0].getAttribute('disabled')).toBeFalsy();
           });

           describe('when valid data and save button is clicked', () =>{
               it('then save method is called', () => {
                   spyOn(controller, 'save');
                   newPropertyMock.address.countryId = countryMockId;
                   scope.$apply();

                   var button = element.find('button#saveBtn');
                   button.click();

                   expect(controller.save).toHaveBeenCalled();
               });

               it('then put request is is called and redirect to view page', () => {
                   var addressFormMock: Dto.AddressForm = new Dto.AddressForm('adrfrmId1', countryMockId, []);
                   $http.whenGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() =>{
                       return [200, addressFormMock];
                   });

                   var propertyFromServerMock: Dto.Property = new Dto.Property();
                   propertyFromServerMock.id = 'propFromServerId1';
                   propertyFromServerMock.address = new Dto.Address();

                   $http.expectPOST(/\/api\/properties/, newPropertyMock).respond(() => {
                       return [200, propertyFromServerMock];
                   });

                   var propertyId: string;
                   spyOn(state, 'go').and.callFake((routeName: string, property: Dto.Property) => {
                       propertyId = property.id;
                   });

                   newPropertyMock.address = new Dto.Address();

                   newPropertyMock.address.id = 'adrId1';
                   newPropertyMock.address.countryId = countryMockId;
                   newPropertyMock.address.addressFormId = 'adrfrmId1';
                   newPropertyMock.address.propertyName = 'test prop name';
                   newPropertyMock.address.propertyNumber = '123456';

                   scope.$apply();

                   var button = element.find('button#saveBtn');
                   button.click();
                   $http.flush();

                   expect(state.go).toHaveBeenCalled();
                   expect(propertyId).toEqual(propertyFromServerMock.id);
               });
           });
        });
    });
}