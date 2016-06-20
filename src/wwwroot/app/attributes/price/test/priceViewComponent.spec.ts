/// <reference path="../../../typings/_all.d.ts" />

module Antares {
	import ControlConfig = Antares.Common.Models.Dto.IControlConfig;

	describe('Given price view control', () =>{
	    var scope: ng.IScope,
	        element: ng.IAugmentedJQuery;

	    var priceMock: number = 1;
		var configMock: ControlConfig = TestHelpers.ConfigGenerator.generateActivityAskingPriceConfig();
	    var schemaMock: Attributes.IPriceControlSchema =
	    {
		    controlId : "mock-price",
		    translationKey : "VIEW.MOCK_PRICE"
	    };

		var pageObjectSelectors = {
            price: '#' + schemaMock.controlId
        };

	    describe('is loaded', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
                scope = $rootScope.$new();
                scope['price'] = priceMock;
                scope['config'] = configMock;
                scope['schema'] = schemaMock;
                element = $compile('<price-view-control price="price" config="config.askingPrice" schema="schema"></price-view-control>')(scope);
				
                scope.$apply();
            }));

            it('then price is displayed', () => {
                var priceElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.price);
	            var expectedPrice: string = priceMock + ".00 GBP";
                expect(priceElement.text()).toBe(expectedPrice);
            });
        });
    });
}