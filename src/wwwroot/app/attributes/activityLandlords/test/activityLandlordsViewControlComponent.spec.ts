/// <reference path="../../../typings/_all.d.ts" />

module Antares {

	import Business = Common.Models.Business;

	describe('Given activity landlords view control', () =>{
		var scope: ng.IScope,
		    element: ng.IAugmentedJQuery,
		    compile: ng.ICompileService;

		var pageObjectSelectors = {
            list: 'list#activity-landlords-view',
            header: 'list#activity-landlords-view list-header',
            noItems: 'list#activity-landlords-view list-no-items',
            items: 'list#activity-landlords-view list-item',
            item: 'list#activity-landlords-view list-item#list-item-'
        };

	    describe('is configured', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {
				
				scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when no landlords then "no items" element should be visible', () => {
                // arrange
	            var contactsMock: Business.Contact[] = TestHelpers.ContactGenerator.generateMany(0);
	            var configMock : Attributes.IActivityLandlordsControlConfig = TestHelpers.ConfigGenerator.generateActivityLandlordsConfig();

	            scope['contacts'] = contactsMock;
                scope['config'] = configMock;

                // act
                element = compile('<activity-landlords-view-control contacts="contacts" config="config"></activity-landlords-view-control')(scope);
                scope.$apply();
              
                // assert
                var noItemsElement = element.find(pageObjectSelectors.noItems);
                var listItemElements = element.find(pageObjectSelectors.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(listItemElements.length).toBe(0);
            });

            it('when landlords exist then the list should be visible', () => {
                // arrange
	            var contactsMock: Business.Contact[] = TestHelpers.ContactGenerator.generateMany(2);
	            var configMock : Attributes.IActivityLandlordsControlConfig = TestHelpers.ConfigGenerator.generateActivityLandlordsConfig();

	            scope['contacts'] = contactsMock;
                scope['config'] = configMock;

                // act
                element = compile('<activity-landlords-view-control contacts="contacts" config="config"></activity-landlords-view-control')(scope);
                scope.$apply();
              
                // assert
                var noItemsElement = element.find(pageObjectSelectors.noItems);
                var listItemElements = element.find(pageObjectSelectors.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(listItemElements.length).toBe(2);
            });

            it('when landlords exist then list item components should have proper data', () => {
                // arrange
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();
	            var configMock : Attributes.IActivityLandlordsControlConfig = TestHelpers.ConfigGenerator.generateActivityLandlordsConfig();
	            scope['contacts'] = [contact1Mock, contact2Mock];
                scope['config'] = configMock;

                // act
                element = compile('<activity-landlords-view-control contacts="contacts" config="config"></activity-landlords-view-control')(scope);
                scope.$apply();

                // assert
                var landlordsItemsElement1 = element.find(pageObjectSelectors.item + contact1Mock.id);
                var landlordsItemsElement2 = element.find(pageObjectSelectors.item + contact2Mock.id);

                expect(landlordsItemsElement1[0].innerText.trim()).toBe(contact1Mock.getName());
                expect(landlordsItemsElement2[0].innerText.trim()).toBe(contact2Mock.getName());
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
                element = compile('<activity-landlords-view-control contacts="contacts" config="config"></activity-landlords-view-control')(scope);
                scope.$apply();
              
                // assert
                var list = element.find(pageObjectSelectors.list);
                expect(list.length).toBe(0);
            });
		});
    });
}