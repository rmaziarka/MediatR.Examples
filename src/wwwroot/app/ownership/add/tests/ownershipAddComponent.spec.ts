/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OwnershipAddController = Antares.Property.OwnershipAddController;
    describe('Given ownership is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: Antares.TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService;

        var pageObjectSelectors = {
            ownershipTypeSelector: 'select#type',
            purchaseDateSelector: '[name=purchaseDate]',
            sellDateSelector: '[name=sellDate]',
            buyPriceSelector: '[name=buyPrice]',
            sellPriceSelector: '[name=sellPrice]'
        };

        var controller: OwnershipAddController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            $http.whenGET(/\/api\/enums\/OwnershipType\/items/).respond(() => {
                return [200, { "items": [{ "id": "1", "value": "Freeholder" }, { "id": "2", "value": "Leaseholder" }] }];
            });

            scope = $rootScope.$new();
            scope["ownerships"] = [];
            element = $compile('<ownership-add ownerships="ownerships"></ownership-add>')(scope);
            $httpBackend.flush();
            scope.$apply();

            controller = element.controller('ownershipAdd');

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));

        describe('when', () => {
            describe('ownership type value is ', () => {
                it('missing then required message should be displayed', () => {
                    assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.ownershipTypeSelector);
                });

                it('not missing then required message should not be displayed', () => {
                    assertValidator.assertRequiredValidator('1', true, pageObjectSelectors.ownershipTypeSelector);                    
                });
            });
            describe('ownership purchasing date is ', () => {
                it('invalid then required message should be displayed', () => {
                    assertValidator.assertRequiredValidator('invalid date', false, pageObjectSelectors.purchaseDateSelector);
                });

                it('vaild then required message should not be displayed', () => {
                    assertValidator.assertRequiredValidator('21-12-1984', true, pageObjectSelectors.purchaseDateSelector);
                });
            });
            describe('ownership selling date is ', () => {

                it('ownership selling date is invalid then required message should be displayed', () => {
                    assertValidator.assertRequiredValidator('invalid date', false, pageObjectSelectors.sellDateSelector);
                });

                it('ownership selling date is valid then required message should not be displayed', () => {
                    assertValidator.assertRequiredValidator('21-12-1984', true, pageObjectSelectors.sellDateSelector);
                });
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
        });
    });
}