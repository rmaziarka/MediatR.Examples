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

    describe('Given activity type preview component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NegotiatorsController,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelector = {
            typePreview: '#activity-preview-type'
        };

        var mockedComponentHtml = '<activity-type-preview-control config="config" type-id="typeId"></activity-type-preview-control>';

        var divisionCodes = [
            { id: "residentialId", code: "RESIDENTIAL" },
            { id: "commmercialId", code: "COMMERCIAL" }
        ];

        var config: Attributes.IActivityTypeEditControlConfig = TestHelpers.ConfigGenerator.generateActivityTypeEditConfig();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {
            
            // init
            scope = <IScope>$rootScope.$new();
            scope['typeId'] = 'typeId';
            scope['config'] = config;
            enumService.setEnum(Dto.EnumTypeCode.Division.toString(), divisionCodes);

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('activityTypePreviewControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            it('with config it should display type', () => {
                var activityType = element.find(pageObjectSelector.typePreview);
                expect(activityType.length).toEqual(1);
            });

            it('type should have proper translation', () => {
                var activityType = element.find(pageObjectSelector.typePreview);
                expect(activityType.text()).toEqual('DYNAMICTRANSLATIONS.typeId');
            });

            it('without config it should not display type', () => {
                scope['config'] = undefined;
                scope.$apply();
                var activityType = element.find(pageObjectSelector.typePreview);
                expect(activityType.length).toEqual(0);
            });
        });
    });
}