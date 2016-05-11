///<reference path="../../../../typings/_all.d.ts"/>

module Antares {
    import BusyController = Antares.Common.Component.BusyController;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given busy component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: BusyController,
            createComponent = (isBusy: boolean, label: string) => { };

        var pageObjectSelector = {
            busy: '.busy',
            spinnerLabel: '.spinner-label'
        };


        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();

            createComponent = (isBusy: boolean, label: string) => {
                element = $compile('<busy is-busy="' + isBusy + '" label="' + label + '"></busy>')(scope);
                scope.$apply();
                controller = element.controller('busy');

            };
        }));


        it('when flag "isBusy" is set to "true" then spinner should appear', () => {
            // arrange
            var isBusy = true,
                label = 'spinner label';
            var panel: ng.IAugmentedJQuery,
                spinnerLabel: ng.IAugmentedJQuery;

            // act
            createComponent(isBusy, label);
            panel = element.find(pageObjectSelector.busy);
            spinnerLabel = panel.find(pageObjectSelector.spinnerLabel);

            // assert
            expect(controller.isBusy).toBe(isBusy);
            expect(controller.label).toBe(label);
            expect(spinnerLabel).toBeDefined();
            expect(spinnerLabel.text()).toBe(label);
        });


        it('when flag "isBusy" is set to "false" then spinner shouldn\'t appear', () => {
            // arrange
            var isBusy = false,
                label = 'spinner label';

            // act
            createComponent(isBusy, label);

            // assert
            expect(controller.isBusy).toBe(isBusy);
            expect(controller.label).toBe(label);
            expect(element.context).toBeUndefined();
        });


    });
}