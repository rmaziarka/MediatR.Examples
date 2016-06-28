/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import EnumItemEditControlController = Attributes.EnumItemEditControlController;

    describe('Given enum item edit component', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: EnumItemEditControlController,

            assertValidator: TestHelpers.AssertValidators;

        var statusMock: number = 1;
        var statusChangedMock = () => { };
        var pageObjectSelector = {
            status: '[name="enumItem"]'
        };

        var mockedComponentHtml = '<enum-item-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></enum-item-edit-control>';

        var activityStatusCodes = [
            { id: "status1", code: "status1" },
            { id: "status2", code: "status2" }
        ];

        var configMock: Attributes.IActivityStatusEditControlConfig = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();
        var schemaMock: Attributes.IEnumItemEditControlSchema = {
            fieldName: 'activityStatusId',
            formName: 'formName',
            controlId: 'controlId',
            enumTypeCode: Dto.EnumTypeCode.ActivityStatus,
            translationKey: 'translationKey'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumProvider: Antares.Providers.EnumProvider) => {
            
            // init
            scope = $rootScope.$new();
            scope['vm'] = { ngModel: statusMock, config: configMock, schema: schemaMock };
            enumProvider.enums = <Dto.IEnumDictionary>{
                activityStatus: activityStatusCodes
            };

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
        });
    });
}