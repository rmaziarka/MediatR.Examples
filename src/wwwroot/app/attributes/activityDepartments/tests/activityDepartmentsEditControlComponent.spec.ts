/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import IActivityViewConfig = Activity.IActivityViewConfig;
    import ActivityDepartmentsEditControlController = Antares.Attributes.ActivityDepartmentsEditControlController;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiators: Business.ActivityUser[];
        propertyDivisionId: string;
    }

    describe('Given activity department edit component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: ActivityDepartmentsEditControlController,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelector = {
            departmentsList: 'card-list'
        };

        var mockedComponentHtml = '<activity-departments-edit-control config="config" activityDepartments="activityDepartments"></activity-departments-edit-control>';

        var departmentCodes = [
            { id: "department1", code: "department1" },
            { id: "department2", code: "department2" }
        ];

        var configMock = { negotiators: {} } as IActivityViewConfig;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {

            // init
            scope = <IScope>$rootScope.$new();
            scope['config'] = configMock;
            enumService.setEnum(Dto.EnumTypeCode.Division.toString(), departmentCodes);

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('activityDepartmentsViewControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            it('with config it should display department list', () => {
                var activityStatus = element.find(pageObjectSelector.departmentsList);
                expect(activityStatus.length).toEqual(1);
            });

            it('without config it should not display department list', () => {
                scope['config'] = undefined;
                scope.$apply();
                var activityStatus = element.find(pageObjectSelector.departmentsList);
                expect(activityStatus.length).toEqual(0);
            });
        });
    });
}