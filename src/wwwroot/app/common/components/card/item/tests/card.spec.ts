/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CardController = Antares.Common.Component.CardController;

    describe('Given card component', () =>{

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: CardController,
            panel: ng.IAugmentedJQuery;

        var mockedTemplateUrl = 'app/common/components/card/item/tests/testCardTemplate.html',
            itemMock = { itemName: 'Test Name', test: 123 },
            showItemDetailsSpy = jasmine.createSpy('showItemDetails');

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) =>{

            // init
            scope = $rootScope.$new();
            compile = $compile;

            scope = $rootScope.$new();
            scope['item'] = itemMock;
            scope['showItemDetails'] = showItemDetailsSpy;
            element = $compile('<card card-template-url="' + "'" + mockedTemplateUrl + "'" + '" item="item" show-item-details="showItemDetails"></card>')(scope);
            scope.$apply();
            controller = element.controller('card');

            panel = element.find('.panel-body');
        }));

        it('when loaded then card body should be visible', () => {
            expect(panel.length).toBe(1);
        });

        it('when loaded then template within card body should be visible', () => {
            var templateElement = panel.find('div.test-card-template-element');
            expect(templateElement.length).toBe(1);
        });

        it('when loaded then template within card body should have binded item', () => {
            var templateElement = panel.find('div.test-card-template-element');
            var spanElement = templateElement.find('span');

            expect(spanElement.text()).toBe('Test Name');
        });

        it('when deatails link clicked then showItemDetails method should be called', () =>{
            var link = panel.find('a#detailsLink');
            link.click();

            expect(showItemDetailsSpy).toHaveBeenCalledWith(itemMock);
        });

    });
}