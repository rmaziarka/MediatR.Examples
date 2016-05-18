/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OwnershipAddController = Property.OwnershipAddController;
    import Dto = Common.Models.Dto;
    import runDescribe = TestHelpers.runDescribe;
    type TestCaseForRequiredValidator = [string, boolean];

    describe('Given ownership is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService;

        var pageObjectSelectors = {
            ownershipTypeSelector: 'enum-select#type select',
            purchaseDateSelector: '[name=purchaseDate]',
            sellDateSelector: '[name=sellDate]',
            buyPriceSelector: '[name=buyPrice]',
            sellPriceSelector: '[name=sellPrice]',
            currentOwnerSelector: '#current-owner-checkbox'
        };

        var ownershipTypes = [
            { "id" : "1", "code" : "Freeholder" },
            { "id" : "2", "code" : "Leaseholder" }
        ];

        var controller: OwnershipAddController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;

            // enums
            enumService.setEnum(Dto.EnumTypeCode.OwnershipType.toString(), ownershipTypes);

            scope = $rootScope.$new();
            scope["ownerships"] = [];
            element = $compile('<ownership-add ownerships="ownerships"></ownership-add>')(scope);
            scope.$apply();

            controller = element.controller('ownershipAdd');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when', () => {
            // RequiredValidator for ownership type
            runDescribe('ownership type ')
                .data<TestCaseForRequiredValidator>([
                    [ownershipTypes[0].id, true],
                    ['typeIdThatIsNotOnTheList', false],
                    ['', false],
                    [null, false]])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) => {
                    // arrange / act / assert
                    assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.ownershipTypeSelector);
                });

            // RequiredValidator for ownership purchasing date
            runDescribe('ownership purchasing date ')
                .data<TestCaseForRequiredValidator>([
                    ['21-12-1984', true],
                    ['', true]])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) => {
                    // arrange / act / assert
                    assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.purchaseDateSelector);
                });

            // RequiredValidator for ownership selling date
            runDescribe('ownership selling date ')
                .data<TestCaseForRequiredValidator>([
                    ['21-12-1984', true],
                    ['', true]])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    `value is "${data[0]}" then required message should ${data[1] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) => {
                    // arrange / act / assert
                    assertValidator.assertRequiredValidator(data[0], data[1], pageObjectSelectors.sellDateSelector);
                });

            describe('ownership buying price is ', () => {
                it('lower then zero then validation message should be displayed', () => {
                    assertValidator.assertMinValueValidator(-10, false, pageObjectSelectors.buyPriceSelector);
                });

                it('in exponential notation then validation message should be displayed', () => {
                    assertValidator.assertPatternValidator('10e2', false, pageObjectSelectors.buyPriceSelector);
                });

                it('valid positive number then no validation message should be displayed', () => {
                    assertValidator.assertRequiredValidator('123', true, pageObjectSelectors.buyPriceSelector);
                });

                it('invalid string input then element should be empty and no validation message is displayed', () => {
                    assertValidator.assertRequiredValidator('xyz', true, pageObjectSelectors.buyPriceSelector);
                    var sellPriceElement = element.find(pageObjectSelectors.buyPriceSelector);
                    expect(sellPriceElement.hasClass('ng-empty')).toBeTruthy();
                });
            });
            describe('ownership sell price is ', () => {
                it('lower then zero then validation message should be displayed', () => {
                    assertValidator.assertMinValueValidator(-10, false, pageObjectSelectors.sellPriceSelector);
                });

                it('in exponential notation then validation message should be displayed', () => {
                    assertValidator.assertPatternValidator('10e2', false, pageObjectSelectors.sellPriceSelector);
                });

                it('valid positive number then no validation message should be displayed', () => {
                    assertValidator.assertRequiredValidator('123', true, pageObjectSelectors.sellPriceSelector);
                });

                it('invalid string input then element should be empty and no validation message is displayed', () => {
                    assertValidator.assertRequiredValidator('xyz', true, pageObjectSelectors.sellPriceSelector);
                    var sellPriceElement = element.find(pageObjectSelectors.sellPriceSelector);
                    expect(sellPriceElement.hasClass('ng-empty')).toBeTruthy();
                });
            });
            
            it('current owner is set then sell price and date should be empty', () => {
                //arrange
                var sellDate = element.find(pageObjectSelectors.sellDateSelector);
                var sellPrice = element.find(pageObjectSelectors.sellPriceSelector);
                sellDate.val('2001-01-01');
                sellPrice.val(100);
                // act
                element.find(pageObjectSelectors.currentOwnerSelector).prop('checked', true);
                // assert
                expect(sellDate.text()).toBe('');    
                expect(sellPrice.text()).toBe('');                 
            });            
        });
    });
}