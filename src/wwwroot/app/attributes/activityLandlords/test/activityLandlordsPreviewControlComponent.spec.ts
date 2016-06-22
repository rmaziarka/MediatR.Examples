/// <reference path="../../../typings/_all.d.ts" />

module Antares {

	import Business = Common.Models.Business;

	describe('Given activity landlords preview control', () =>{
		var scope: ng.IScope,
		    element: ng.IAugmentedJQuery,
		    compile: ng.ICompileService;

		var pageObjectSelectors = {
			control: '#activity-landlords-preview',
            item: '#activity-landlords-preview-item-'
        };

	    describe('is configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
				scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when landlords exists then list should be displayed', () => {
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();
	            var configMock : Attributes.IActivityLandlordsControlConfig = TestHelpers.ConfigGenerator.generateActivityLandlordsConfig();

	            scope['contacts'] = [contact1Mock, contact2Mock];
                scope['config'] = configMock;

                // act
                element = compile('<activity-landlords-preview-control contacts="contacts" config="config"></activity-landlords-previewv-control')(scope);
                scope.$apply();

                // assert
                var landlordsItemsElement1 = element.find(pageObjectSelectors.item + contact1Mock.id);
                var landlordsItemsElement2 = element.find(pageObjectSelectors.item + contact2Mock.id);

                expect(landlordsItemsElement1[0].innerText).toBe(contact1Mock.getName());
                expect(landlordsItemsElement2[0].innerText).toBe(contact2Mock.getName());
            });
        });

		describe('is not configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
				scope = $rootScope.$new();
                compile = $compile;
            }));

			it('then the control should not be visible', () => {
                // arrange
	            var contactsMock: Business.Contact[] = TestHelpers.ContactGenerator.generateMany(3);
	            scope['contacts'] = contactsMock;

                // act
                element = compile('<activity-landlords-preview-control contacts="contacts" config="config"></activity-landlords-preview-control')(scope);
                scope.$apply();
              
                // assert
                var controlElement = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
		});
    });
}