/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import RequirementCardController = Common.Component.RequirementCardController;

    describe('Given requirement card controller ', () => {
        var state: ng.ui.IStateService,
            controller: RequirementCardController,
            requirementMock = TestHelpers.RequirementGenerator.generate(
                { contacts: TestHelpers.ContactGenerator.generateMany(3) });

        beforeEach(inject((
            $controller: ng.IControllerService,
            $state: ng.ui.IStateService) => {

            state = $state;
            var bindings: any = { requirement: requirementMock };
            controller = <RequirementCardController>$controller('requirementCardController', {}, bindings);
            controller.requirement = requirementMock;
        }));

        it('when navigateToRequirment() is called then user navigates to requirement page', () => {
            // arrange
            spyOn(state, 'go');

            // act
            controller.navigateToRequirement(requirementMock);

            // assert
            expect(state.go).toHaveBeenCalledWith('app.requirement-view', { id: requirementMock.id });
        });
    });
}