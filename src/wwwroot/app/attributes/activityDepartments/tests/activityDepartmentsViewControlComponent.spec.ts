/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import IActivityViewConfig = Activity.IActivityViewConfig;
    import ActivityDepartmentsViewControlController = Antares.Attributes.ActivityDepartmentsViewControlController;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiators: Business.ActivityUser[];
        propertyDivisionId: string;
    }

    describe('Given activity negotiators view component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: ActivityDepartmentsViewControlController,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelector = {
            departmentsList: 'card-list'
        };

        var mockedComponentHtml = '<activity-departments-view-control config="config" activityDepartments="activityDepartments"></activity-departments-view-control>';

        var divisionCodes = [
            { id: "residentialId", code: "RESIDENTIAL" },
            { id: "commmercialId", code: "COMMERCIAL" }
        ];

        var configMock = { negotiators: {} } as IActivityViewConfig;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {

            // init
            scope = <IScope>$rootScope.$new();
            scope['config'] = configMock;

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