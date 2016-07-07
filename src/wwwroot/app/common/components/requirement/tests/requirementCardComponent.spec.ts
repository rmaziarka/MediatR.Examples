/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    describe('Given requirement card component ', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: Common.Component.RequirementCardController,
            requirementMock = TestHelpers.RequirementGenerator.generate(
            { contacts : TestHelpers.ContactGenerator.generateMany(3) });

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) =>{

            scope = $rootScope.$new();

            element = $compile('<requirement-card></requirement-card>')(scope);
            scope.$apply();

            controller = element.controller('requirementCard');
            controller.requirement = requirementMock;

            scope["requirement"] = requirementMock;
            scope.$apply();
        }));

        it('then requirement template should containt contact names', () =>{
            // assert
            var requirementDetailsElement = element.find('card-template');
            var properRequirementTextToDisplay = controller.requirement.getContactNames();
            expect(requirementDetailsElement.text()).toBe(properRequirementTextToDisplay);
        });
    });
}