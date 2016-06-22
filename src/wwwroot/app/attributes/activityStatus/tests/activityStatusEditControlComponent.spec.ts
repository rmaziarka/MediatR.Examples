/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import ActivityStatusEditControlController = Attributes.ActivityStatusEditControlController;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiators: Business.ActivityUser[];
        propertyDivisionId: string;
    }

    describe('Given activity status edit component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: ActivityStatusEditControlController,
            assertValidator: TestHelpers.AssertValidators;

        var statusMock: number = 1;
        var statusChangedMock = () => { };
        var pageObjectSelector = {
            status: '#status'
        };

        var mockedComponentHtml = '<activity-status-control ng-model="vm.ngModel" config="vm.config" on-activity-status-changed="vm.statusChanged()"></activity-status-control>';

        var activityStatusCodes = [
            { id: "status1", code: "status1" },
            { id: "status2", code: "status2" }
        ];

        var configMock: Attributes.IActivityStatusEditControlConfig = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {
            
            // init
            scope = <IScope>$rootScope.$new();
            scope['vm'] = { ngModel: statusMock, config: configMock, statusChanged: statusChangedMock };
            spyOn(scope['vm'], 'statusChanged');
            enumService.setEnum(Dto.EnumTypeCode.ActivityStatus.toString(), activityStatusCodes);

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('activityStatusControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            it('with config it should display status', () => {
                var activityStatus = element.find(pageObjectSelector.status);
                expect(activityStatus.length).toEqual(1);
            });

            it('without config it should not display status', () => {
                scope['vm']['config'] = undefined;
                scope.$apply();
                var activityStatus = element.find(pageObjectSelector.status);
                expect(activityStatus.length).toEqual(0);
            });

            it('verify select attributes', () => {
                scope['vm']['config'] = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();
                scope['vm']['config'].activityStatusId = { required: true, active: true };
                scope.$apply();
                var activityStatus = element.find(pageObjectSelector.status);
                expect((<HTMLSelectElement>activityStatus[0]).disabled).toEqual(false);
                expect((<HTMLSelectElement>activityStatus[0]).required).toEqual(true);
            });

            it('when select value is change then on change method is called', () => {
                scope['vm']['config'] = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();

                controller.ngModel = 'status2';
                scope.$apply();
                element.find(pageObjectSelector.status).triggerHandler('change');
                expect(scope['vm']['statusChanged']).toHaveBeenCalled();
            });
        });
    });
}