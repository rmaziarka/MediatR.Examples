/// <reference path="../../../typings/_all.d.ts" />

module Antares {
	describe('Given price edit control', () =>{
		var scope: ng.IScope,
		    element: ng.IAugmentedJQuery,
		    assertValidator: TestHelpers.AssertValidators;

	    var priceMock: number = 1;
        var configMock = { askingPrice: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()};
	    var schemaMock: Attributes.IPriceEditControlSchema =
	    {
		    controlId : "mock-price",
		    translationKey : "EDIT.MOCK_PRICE",
		    formName : "mockPriceControlForm",
			fieldName: "askingPrice"
	    };

		var pageObjectSelectors = {
			control: "#price-edit-control",
            price: '#' + schemaMock.controlId
        };


	    describe('is configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
                scope = $rootScope.$new();
	            scope['vm'] = { ngModel : priceMock, config : configMock, schema : schemaMock };
                element = $compile('<price-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></price-edit-control>')(scope);
                scope.$apply();

				assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('then price is displayed', () => {
                var priceElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.price);
                expect(priceElement.val()).toBe(priceMock.toString());
            });

			it('when price value is required then validation message should be displayed', () => {
				assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.price);
            });

            it('when price value is lower than minimum value then validation message should be displayed', () => {
                var minValue = 0;
                assertValidator.assertNumberGreaterThenValidator(minValue, false, pageObjectSelectors.price);
            });
        });

		describe('is not configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
                scope = $rootScope.$new();
	            scope['vm'] = { ngModel : priceMock, schema : schemaMock };
                element = $compile('<price-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></price-edit-control>')(scope);
                scope.$apply();
            }));

            it('then control is not displayed', () => {
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
        });
    });
}