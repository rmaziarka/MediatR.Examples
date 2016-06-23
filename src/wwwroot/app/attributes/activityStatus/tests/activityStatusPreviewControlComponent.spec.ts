/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import NegotiatorsController = Common.Component.ActivityNegotiatorsViewControlController;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiators: Business.ActivityUser[];
        propertyDivisionId: string;
    }

    describe('Given activity status preview component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NegotiatorsController,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelector = {
            statusPreview: '#activity-preview-status'
        };

        var mockedComponentHtml = '<activity-status-preview-control config="config" status-id="statusId"></activity-status-preview-control>';
        

        var config: Attributes.IActivityStatusEditControlConfig = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {
            
            // init
            scope = <IScope>$rootScope.$new();
            scope['statusId'] = 'statusId';
            scope['config'] = config;

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('activityStatusPreviewControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            it('with config it should display status', () => {
                var activityStatus = element.find(pageObjectSelector.statusPreview);
                expect(activityStatus.length).toEqual(1);
            });

            it('status should have proper translation', () => {
                var activityStatus = element.find(pageObjectSelector.statusPreview);
                expect(activityStatus.text()).toEqual('DYNAMICTRANSLATIONS.statusId');
            });

            it('without config it should not display status', () => {
                scope['config'] = undefined;
                scope.$apply();
                var activityStatus = element.find(pageObjectSelector.statusPreview);
                expect(activityStatus.length).toEqual(0);
            });
        });
    });
}