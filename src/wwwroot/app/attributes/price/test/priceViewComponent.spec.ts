/// <reference path="../../../typings/_all.d.ts" />

module Antares {
	describe('Given price view control', () =>{
	    var scope: ng.IScope,
	        element: ng.IAugmentedJQuery;

	    var priceMock: number = 1;
        var configMock: Antares.Common.Models.Dto.IFieldConfig = TestHelpers.ConfigGenerator.generateFieldConfig();
	    var schemaMock: Attributes.IPriceControlSchema =
	    {
		    controlId : "mock-price",
		    translationKey : "VIEW.MOCK_PRICE"
	    };

		var pageObjectSelectors = {
			control: "#price-view-control",
            price: '#' + schemaMock.controlId
        };

	    describe('is configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
                scope = $rootScope.$new();
                scope['price'] = priceMock;
                scope['config'] = configMock;
                scope['schema'] = schemaMock;
                element = $compile('<price-view-control price="price" config="config" schema="schema"></price-view-control>')(scope);
				
                scope.$apply();
            }));

            it('then price is displayed', () => {
                var priceElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.price);
	            var expectedPrice: string = priceMock + ".00 GBP";
                expect(priceElement.text()).toBe(expectedPrice);
            });
        });

		describe('is not configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
                scope = $rootScope.$new();
                scope['price'] = priceMock;
                scope['schema'] = schemaMock;
                element = $compile('<price-view-control price="price" config="config" schema="schema"></price-view-control>')(scope);
				
                scope.$apply();
            }));

            it('then control is not displayed', () => {
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
        });
    });
}