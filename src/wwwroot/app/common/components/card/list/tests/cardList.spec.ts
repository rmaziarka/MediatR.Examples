/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CardListController = Antares.Common.Component.CardListController;

    describe('Given card list component', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: CardListController;

        var mockedComponentHtml = '<card-list show-item-add="showItemAdd">'
                + '<card-list-header><p id="test-header">Header test</p></card-list-header>'
                + '<card-list-no-items><p id="test-no-items">Test</p></card-list-no-items>'
                + '<card-list-item><p id="test-item">Test</p></card-list-item>'
                + '</card-list>';

        var showItemAddSpy = jasmine.createSpy('showItemAdd');

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            // init
            scope = $rootScope.$new();
            compile = $compile;

            scope['showItemAdd'] = showItemAddSpy;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('cardList');
        }));

        it('when loaded then all component transcluded elements should be visible', () => {
            var headerElement = element.find('p#test-header');
            var noItemsElement = element.find('p#test-no-items');
            var itemElement = element.find('p#test-item');

            expect(headerElement.length).toBe(1);
            expect(noItemsElement.length).toBe(1);
            expect(itemElement.length).toBe(1);
        });

        it('when add button clicked then showItemAdd method should be called', () => {
            var button = element.find('button#addItemBtn');
            button.click();

            expect(showItemAddSpy).toHaveBeenCalled();
        });
    });
}