///<reference path="../../../../../typings/main.d.ts"/>

module Antares {
    import SidePanelController = Antares.Common.Component.SidePanelController;

    describe('Given side panel is rendered', () =>{

        var scope: ng.IScope,
            element: ng.IAugmentedJQuery;

        var sidePanelContent = 'main content';
        var sidePanelFooter = 'footer content';

        var controller: SidePanelController;
        var panel: ng.IAugmentedJQuery;

        var assertCssClasses = (isVisible: boolean) => {
            expect(panel.hasClass('slide-in')).toBe(isVisible);
            expect(panel.hasClass('slide-out')).toBe(!isVisible);
        }

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) =>{

            scope = $rootScope.$new();
            element = $compile('<side-panel><side-panel-content>' + sidePanelContent + '</side-panel-content><side-panel-footer>' + sidePanelFooter + '</side-panel-footer></side-panel>')(scope);
            scope.$apply();

            controller = element.controller('sidePanel');
            panel = element.find('.slide-panel');
        }));

        it('side panel should be hidden on init', () => {
            expect(panel.length).toBe(1);
            expect(controller.visible).toBe(false);
            expect(controller.stateChanged).toBe(false);

            // on init we don't set any of classes tested below
            expect(panel.hasClass('slide-in')).toBe(false);
            expect(panel.hasClass('slide-out')).toBe(false);
        });

        it('side panel should be visible when show() is called', () =>{
            controller.show();
            scope.$apply();

            expect(controller.visible).toBe(true);
            expect(controller.stateChanged).toBe(true);

            assertCssClasses(true);
        });

        it('side panel should be hidden when hide() is called', () =>{
            controller.show();
            scope.$apply();
            controller.hide();
            scope.$apply();

            expect(controller.visible).toBe(false);
            expect(controller.stateChanged).toBe(true);
            
            assertCssClasses(false);
        });

        it('side panel should remain hidden when it is already hidden and hide() is called', () =>{
            controller.hide();
            scope.$apply();

            expect(controller.visible).toBe(false);
            expect(controller.stateChanged).toBe(true);

            assertCssClasses(false);
        });

        it('side panel should remain visible when it is already shown and show() is called', () => {
            controller.show();
            scope.$apply();
            controller.show();
            scope.$apply();

            expect(controller.visible).toBe(true);
            expect(controller.stateChanged).toBe(true);

            assertCssClasses(true);
        });
    });
}