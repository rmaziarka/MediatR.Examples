/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CardListController = Antares.Common.Component.CardListController;

    describe('Given card list component', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: CardListController;

        var mockedComponentHtml = '<card-list items="items" show-item-add="showItemAdd">'
                + '<card-list-header><p id="test-header">Header test</p></card-list-header>'
                + '<card-list-no-items><p id="test-no-items">Test</p></card-list-no-items>'
                + '</card-list>';

        var itemsMock = [{ itemName: 'It1', test: 123 }, { itemName: 'It2', test: 456 }],
            showItemAddSpy = jasmine.createSpy('showItemAdd');

        beforeEach(angular.mock.module('app'));
        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            // init
            scope = $rootScope.$new();
            compile = $compile;

            scope['showItemAdd'] = showItemAddSpy;
        }));

        it('when loaded then all component transcluded elements should be visible', () => {
            scope['items'] = [];

            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('cardList');

            var headerElement = element.find('p#test-header');
            var noItemsElement = element.find('p#test-no-items');

            expect(headerElement.length).toBe(1);
            expect(noItemsElement.length).toBe(1);
        });

        it('when loaded and no items then "no items" element should be visible', () => {
            scope['items'] = [];

            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('cardList');

            var noItemsElement = element.find('div#no-items-element');
            var cardElements = element.find('card');

            expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
            expect(cardElements.length).toBe(0);
        });

        it('when loaded and existing items then card components should be visible', () => {
            scope['items'] = itemsMock;

            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('cardList');

            var noItemsElement = element.find('div#no-items-element');
            var cardElements = element.find('card');

            expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
            expect(cardElements.length).toBe(2);
        });

        it('when add button clicked then showItemAdd method should be called', () => {
            scope['items'] = itemsMock;

            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('cardList');

            var button = element.find('button#addItemBtn');
            button.click();

            expect(showItemAddSpy).toHaveBeenCalled();
        });
    });
}