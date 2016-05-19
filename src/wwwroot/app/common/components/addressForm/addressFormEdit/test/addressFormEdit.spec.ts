/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AddressFormEditController = Common.Component.AddressFormEditController;
    import Business = Common.Models.Business;

    describe('Given address component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            controller: AddressFormEditController;

        var countriesMock = [
                { country : { id : "countryId1", isoCode : "GB" }, locale : {}, value : "United Kingdom" },
                { country: { id: "countryId2", isoCode: "TESTCOUNTRY" }, locale : {}, value : "Test Country" }
            ],
            countryMockId = countriesMock[0].country.id,
            addressMock: Business.Address = new Business.Address();

        addressMock.id = 'adrId1';
        addressMock.countryId = countryMockId;
        addressMock.addressFormId = 'adrfrmId1';
        addressMock.propertyName = 'test prop name';
        addressMock.propertyNumber = '123456';

        describe('when component is being loaded', () =>{
            var addressFormMock = new Business.AddressForm('adrfrmId1', countryMockId, []);

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
                $http.expectGET(/\/api\/resources\/countries\/addressform\?entityTypeItemCode=Property/).respond(() =>{
                    return [200, countriesMock];
                });
            }));

            it('and address is empty then user country is set for address and proper request GET for address form is called', () => {
                $http.expectGET(/\/api\/addressForms\/\?entityType=Property&countryCode=TESTCOUNTRY/).respond(() =>{
                    return [200, addressFormMock];
                });

                scope['userCountryCode'] = 'TESTCOUNTRY';
                scope['address'] = new Business.Address();
                element = compile('<address-form-edit entity-type-code="' + "'Property'" + '" address="address" user-country-code="userCountryCode"></address-form-edit>')(scope);
                scope.$apply();
                controller = element.controller('addressFormEdit');

                $http.flush();

                expect(controller.address.countryId).toEqual('countryId2');
            });

            it('and address is not empty then request GET for address form is called and proper addressForm returned from request is set', () => {
                $http.expectGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                    return [200, addressFormMock];
                });

                scope['address'] = addressMock;
                element = compile('<address-form-edit entity-type-code="' + "'Property'" + '" address="address"></address-form-edit>')(scope);
                scope.$apply();
                controller = element.controller('addressFormEdit');

                $http.flush();

                expect(controller.addressForm).toEqual(addressFormMock);
            });

            it('and address is not empty then address form is visible', () => {
                $http.expectGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                    return [200, addressFormMock];
                });

                scope['address'] = addressMock;
                element = compile('<address-form-edit entity-type-code="' + "'Property'" + '" address="address"></address-form-edit>')(scope);
                scope.$apply();

                $http.flush();

                var addressFormElement = element.find('ng-form#form-address');
                expect(addressFormElement.hasClass('ng-hide')).toBeFalsy();
            });
        });

        describe('when component has been loaded', () =>{
            var assertValidator: TestHelpers.AssertValidators,
                addressFormMock: Business.AddressForm = new Business.AddressForm('adrfrmId1', countryMockId, [
                    new Business.AddressFormFieldDefinition('OrderedFieldA', 'Label A', false, '', 1, 1, 3),
                    new Business.AddressFormFieldDefinition('OrderedFieldB', 'Label B', true, '^.{1,3}$', 1, 3, 3),
                    new Business.AddressFormFieldDefinition('OrderedFieldC', 'Label C', true, '^[a-z]$', 1, 2, 6),
                    new Business.AddressFormFieldDefinition('OrderedFieldD', 'Label D', false, '', 3, 1, 2),
                    new Business.AddressFormFieldDefinition('OrderedFieldE', 'Label E', true, '', 2, 1, 4)
                ]);

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                $http.whenGET(/\/api\/resources\/countries\/addressform\?entityTypeItemCode=Property/).respond(() => {
                    return [200, countriesMock];
                });
                $http.whenGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                    return [200, addressFormMock];
                });

                // compile
                scope['address'] = addressMock;
                element = compile('<address-form-edit entity-type-code="' + "'Property'" + '" address="address"></address-form-edit>')(scope);
                scope.$apply();
                controller = element.controller('addressFormEdit');
                assertValidator = new TestHelpers.AssertValidators(element, scope);

                $http.flush();
            }));

            it('then address form has proper elements order grouped in rows', () => {
                // 1:       1.1: OrderedFieldA,     1.2: OrderedFieldC,     1.3: OrderedFieldB
                // 2:       1.1: OrderedFieldE
                // 3:       1.1: OrderedFieldD
                var addressFormElement = element.find('ng-form#form-address');
                var fieldAElement = addressFormElement.find('input#orderedFieldA')[0];
                var fieldBElement = addressFormElement.find('input#orderedFieldB')[0];
                var fieldCElement = addressFormElement.find('input#orderedFieldC')[0];
                var fieldDElement = addressFormElement.find('input#orderedFieldD')[0];
                var fieldEElement = addressFormElement.find('input#orderedFieldE')[0];

                var allRowElements = addressFormElement.find('div.row');
                var firstRow = allRowElements.first();
                var secondRow = firstRow.next();
                var thirdRow = secondRow.next();

                var firstRowInputElements = firstRow.find('input[id*="orderedField"]');
                var secondRowInputElements = secondRow.find('input[id*="orderedField"]');
                var thirdRowInputElements = thirdRow.find('input[id*="orderedField"]');

                expect(firstRowInputElements.length).toBe(3);
                expect(firstRowInputElements[0]).toBe(fieldAElement);
                expect(firstRowInputElements[1]).toBe(fieldCElement);
                expect(firstRowInputElements[2]).toBe(fieldBElement);

                expect(secondRowInputElements.length).toBe(1);
                expect(secondRowInputElements[0]).toBe(fieldEElement);

                expect(thirdRowInputElements.length).toBe(1);
                expect(thirdRowInputElements[0]).toBe(fieldDElement);
            });
        });

        describe('and form is being filled', () => {
            var pageObjectSelectors = {
                fieldASelector: 'input#fieldA',
                fieldBSelector: 'input#fieldB',
                fieldCSelector: 'input#fieldC',
                fieldDSelector: 'input#fieldD',
                fieldESelector: 'input#fieldE'
            };

            var assertValidator: TestHelpers.AssertValidators,
                addressFormMock: Business.AddressForm = new Business.AddressForm('adrfrmId1', countryMockId, [
                    new Business.AddressFormFieldDefinition('FieldA', 'Label A', false, '', 1, 1, 3),
                    new Business.AddressFormFieldDefinition('FieldB', 'Label B', true, '^.{1,3}$', 1, 3, 3),
                    new Business.AddressFormFieldDefinition('FieldC', 'Label C', true, '^[a-z]+$', 1, 2, 6),
                    new Business.AddressFormFieldDefinition('FieldD', 'Label D', false, '', 3, 1, 2),
                    new Business.AddressFormFieldDefinition('FieldE', 'Label E', true, '', 2, 1, 4)
                ]);

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                $http.whenGET(/\/api\/resources\/countries\/addressform\?entityTypeItemCode=Property/).respond(() => {
                    return [200, countriesMock];
                });
                $http.whenGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                    return [200, addressFormMock];
                });

                // compile
                scope['address'] = addressMock;
                element = compile('<address-form-edit entity-type-code="' + "'Property'" + '" address="address"></address-form-edit>')(scope);
                scope.$apply();
                controller = element.controller('addressFormEdit');
                assertValidator = new TestHelpers.AssertValidators(element, scope);

                $http.flush();
            }));

            it('when not required value is missing then required message should not be displayed', () => {
                assertValidator.assertRequiredValidator(null, true, pageObjectSelectors.fieldASelector);
            });

            it('when required value is filled then required message should not be displayed', () => {
                assertValidator.assertRequiredValidator('abc', true, pageObjectSelectors.fieldESelector);
            });

            it('when required value is missing then required message should be displayed', () => {
                assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.fieldESelector);
            });

            it('when value does not match pattern then invalid format message should be displayed', () => {
                assertValidator.assertPatternValidator('123', false, pageObjectSelectors.fieldCSelector);
            });

            it('when value matches pattern then invalid format message should not be displayed', () => {
                assertValidator.assertPatternValidator('xyz', true, pageObjectSelectors.fieldCSelector);
            });
        });
    });
}