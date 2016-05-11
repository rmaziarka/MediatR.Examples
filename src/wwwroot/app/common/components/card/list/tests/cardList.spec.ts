/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CardListController = Antares.Common.Component.CardListController;

    describe('Given card list component', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: CardListController;

        var mockedComponentHtml = '<card-list show-item-add="showItemAdd" show-item-add-disabled="showItemAddDisabled">'
            + '<card-list-header><p id="test-header">Header test</p></card-list-header>'
            + '<card-list-no-items><p id="test-no-items">Test</p></card-list-no-items>'
            + '<card-list-item><p id="test-item">Test</p></card-list-item>'
            + '</card-list>';

        var showItemAddSpy = jasmine.createSpy('showItemAdd');

        var pageObjectSelectors = {
            headerElement: 'p#test-header',
            noItemsElement: 'p#test-no-items',
            itemElement: 'p#test-item',
            button: 'button#addItemBtn'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            // init
            scope = $rootScope.$new();
            compile = $compile;

            scope['showItemAdd'] = showItemAddSpy;
            scope['showItemAddDisabled'] = false;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('cardList');
        }));

        it('when loaded then all component transcluded elements should be visible', () => {
            var headerElement = element.find(pageObjectSelectors.headerElement);
            var noItemsElement = element.find(pageObjectSelectors.noItemsElement);
            var itemElement = element.find(pageObjectSelectors.itemElement);

            expect(headerElement.length).toBe(1);
            expect(noItemsElement.length).toBe(1);
            expect(itemElement.length).toBe(1);
        });

        it('when add button clicked then showItemAdd method should be called', () => {
            var button = element.find(pageObjectSelectors.button);
            button.click();

            expect(button.attr('disabled')).toBeFalsy();
            expect(showItemAddSpy).toHaveBeenCalled();
        });

        it('when add button is disabled then should have disbaled attribute', () => {
            scope['showItemAddDisabled'] = true;
            scope.$apply();

            var button = element.find(pageObjectSelectors.button);

            expect(controller.showItemAddDisabled).toBe(true);
            expect(button.attr('disabled')).toBeTruthy();
        });
    });
}