/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    describe('Given activity card component ', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: Common.Component.ActivityCardController,
            activityMock = TestHelpers.ActivityGenerator.generate();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();

            element = $compile('<activity-card></activity-card>')(scope);
            scope.$apply();

            controller = element.controller('activityCard');
            controller.activity = activityMock;

            scope["activity"] = activityMock;
            scope.$apply();
        }));

        it('then activity link should be proper', () => {
            // assert
            var activityLink = element.find('#activity-link').children('a').attr('ui-sref');
            expect(activityLink).toEqual('app.activity-view({id: vm.activity.id})');
        });

        it('then activity details should display proper address text', () => {
            // assert
            var activityDetailsElement = element.find('#activity-details');
            var properActivityTextToDisplay = controller.activity.property.address.getAddressText();
            expect(activityDetailsElement.text()).toBe(properActivityTextToDisplay);
        });
    });
}