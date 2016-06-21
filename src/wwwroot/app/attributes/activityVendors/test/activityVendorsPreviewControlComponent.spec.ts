/// <reference path="../../../typings/_all.d.ts" />

module Antares {

	import Business = Common.Models.Business;

	describe('Given activity vendors preview control', () =>{
		var scope: ng.IScope,
		    element: ng.IAugmentedJQuery,
		    compile: ng.ICompileService;

		var pageObjectSelectors = {
			control: '#activity-vendors-preview',
            item: '#activity-vendors-preview-item-'
        };

	    describe('is configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
				scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when vendors exists then list should be displayed', () => {
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();
	            var configMock : Attributes.IActivityVendorsControlConfig = TestHelpers.ConfigGenerator.generateActivityVendorsConfig();

	            scope['contacts'] = [contact1Mock, contact2Mock];
                scope['config'] = configMock;

                // act
                element = compile('<activity-vendors-preview-control contacts="contacts" config="config"></activity-vendors-previewv-control')(scope);
                scope.$apply();

                // assert
                var vendorsItemsElement1 = element.find(pageObjectSelectors.item + contact1Mock.id);
                var vendorsItemsElement2 = element.find(pageObjectSelectors.item + contact2Mock.id);

                expect(vendorsItemsElement1[0].innerText).toBe(contact1Mock.getName());
                expect(vendorsItemsElement2[0].innerText).toBe(contact2Mock.getName());
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
                element = compile('<activity-vendors-preview-control contacts="contacts" config="config"></activity-vendors-preview-control')(scope);
                scope.$apply();
              
                // assert
                var controlElement = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
		});
    });
}