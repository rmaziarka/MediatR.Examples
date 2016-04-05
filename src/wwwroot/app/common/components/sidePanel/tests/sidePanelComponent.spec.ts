///<reference path="../../../../typings/_all.d.ts"/>

module Antares {
    import SidePanelController = Antares.Common.Component.SidePanelController;


    describe('Given side panel is rendered', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: SidePanelController;

        var pageObjectSelector = {
            footer: '.side-panel-footer',
            header: '#header'
        };

        describe('with empty header and footer', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                element = $compile('<side-panel><side-panel-content></side-panel-content></side-panel>')(scope);
                scope.$apply();
                controller = element.controller('sidePanel');
            }));

            it('then no footer should be rendered', () => {
                expect(element.find(pageObjectSelector.footer).length).toBe(0);
            });

            it('then no header should be rendered', () => {
                expect(element.find(pageObjectSelector.header).length).toBe(0);
            });
        });

        describe('with not empty header and footer', () => {
            var sidePanelContent = 'main content';
            var sidePanelHeader = 'header content';
            var sidePanelFooter = 'footer content';

            var panel: ng.IAugmentedJQuery;

            var assertCssClasses = (isVisible: boolean) => {
                expect(panel.hasClass('slide-in')).toBe(isVisible);
                expect(panel.hasClass('slide-out')).toBe(!isVisible);
            };

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                element = $compile('<side-panel><side-panel-header>' + sidePanelHeader + '</side-panel-header><side-panel-content>' + sidePanelContent + '</side-panel-content><side-panel-footer>' + sidePanelFooter + '</side-panel-footer></side-panel>')(scope);
                scope.$apply();
                controller = element.controller('sidePanel');
                panel = element.find('.side-panel');
            }));

            it('then side panel should be hidden on init', () => {
                expect(panel.length).toBe(1);
                expect(controller.visible).toBe(false);
                expect(controller.stateChanged).toBe(false);

                // on init we don't set any of classes tested below
                expect(panel.hasClass('slide-in')).toBe(false);
                expect(panel.hasClass('slide-out')).toBe(false);
            });

            it('then side panel should be visible when show() is called', () => {
                controller.show();
                scope.$apply();

                expect(controller.visible).toBe(true);
                expect(controller.stateChanged).toBe(true);

                assertCssClasses(true);
            });

            it('then side panel should be hidden when hide() is called', () => {
                controller.show();
                scope.$apply();
                controller.hide();
                scope.$apply();

                expect(controller.visible).toBe(false);
                expect(controller.stateChanged).toBe(true);

                assertCssClasses(false);
            });

            it('then side panel should remain hidden when it is already hidden and hide() is called', () => {
                controller.hide();
                scope.$apply();

                expect(controller.visible).toBe(false);
                expect(controller.stateChanged).toBe(false);

                // when state does not change we don't set any of classes tested below
                expect(panel.hasClass('slide-in')).toBe(false);
                expect(panel.hasClass('slide-out')).toBe(false);
            });

            it('then side panel should remain visible when it is already shown and show() is called', () => {
                controller.show();
                scope.$apply();
                controller.show();
                scope.$apply();

                expect(controller.visible).toBe(true);
                expect(controller.stateChanged).toBe(true);

                assertCssClasses(true);
            });

            it('then footer should be rendered', () => {
                expect(element.find(pageObjectSelector.footer).length).toBe(1);
                var footerElement: HTMLElement = element.find(pageObjectSelector.footer)[0];

                expect(footerElement.innerText).toBe(sidePanelFooter);
            });

            it('then header should be rendered', () => {
                expect(element.find(pageObjectSelector.header).length).toBe(1);
                var headerElement: HTMLElement = element.find(pageObjectSelector.header)[0];

                expect(headerElement.innerText).toBe(sidePanelHeader);
            });
        });
    });
}