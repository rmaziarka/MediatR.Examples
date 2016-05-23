/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CardController = Common.Component.CardController;
    describe('Given card component', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: CardController,
            panel: ng.IAugmentedJQuery;

        var pageObjectSelectors = {
            panelBody: 'div.panel-body',
            template: 'div.test-card-template-element',
            detailsLink: 'a.detailsLink'
        }

        var mockedTemplateUrl = 'app/common/components/card/item/tests/testCardTemplate.html',
            itemMock = { itemName: 'Test Name', test: 123 },
            showItemDetailsSpy = jasmine.createSpy('showItemDetails');

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            // init
            scope = $rootScope.$new();
            compile = $compile;

            scope = $rootScope.$new();
            scope['item'] = itemMock;
            scope['showItemDetails'] = showItemDetailsSpy;
            element = $compile('<card card-template-url="' + "'" + mockedTemplateUrl + "'" + '" item="item" show-item-details="showItemDetails"></card>')(scope);
            scope.$apply();
            controller = element.controller('card');

            panel = element.find(pageObjectSelectors.panelBody);
        }));

        describe('when loaded', () =>{
            it('then card body should be visible', () => {
                // assert
                expect(panel.length).toBe(1);
            });

            it('then template within card body should be visible', () => {
                // assert
                var templateElement = panel.find(pageObjectSelectors.template);
                expect(templateElement.length).toBe(1);
            });

            it('then template within card body should have binded item', () => {
                // assert
                var templateElement = panel.find(pageObjectSelectors.template);
                var spanElement = templateElement.find('span');

                expect(spanElement.text()).toBe('Test Name');
            });

            it('and showItemDetails method is not set then details link should not be visible', () => {
                // arrange / act
                controller.showItemDetails = undefined;
                scope.$apply();

                // assert
                var link = panel.find(pageObjectSelectors.detailsLink);
                expect(link.length).toBe(0);
            });

            it('and deatails link clicked then showItemDetails method should be called', () => {
                // act
                var link = panel.find(pageObjectSelectors.detailsLink);
                link.click();

                // assert
                expect(showItemDetailsSpy).toHaveBeenCalledWith(itemMock);
            });
        });
    });
}