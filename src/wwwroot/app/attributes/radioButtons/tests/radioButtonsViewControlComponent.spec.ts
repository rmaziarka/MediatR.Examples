/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given radio button view control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators;

        var configMock: Common.Models.Dto.IFieldConfig = <Common.Models.Dto.IFieldConfig>{
            active: true,
            required: false
        };

        var schemaMock: Attributes.IRadioButtonsViewControlSchema = {
            controlId: "offerContractApproved",
            translationKey: "OFFER.EDIT.CONTRACT_APPROVED",
            templateUrl: "app/attributes/radioButtons/templates/radioButtonsViewBoolean.html"
        }

        var pageObjectSelectors = {
            status: '.status'
        };
        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            scope['vm'] = { config: configMock, schema: schemaMock, model: {} };
            element = $compile('<radio-buttons-view-control config="vm.config" schema="vm.schema" model=""></radio-buttons-edit-control>')(scope);
            scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when config is provided', () => {
            it('then form is displayed', () => {
                var form: ng.IAugmentedJQuery = element.find(pageObjectSelectors.status);
                expect(form.length).toEqual(1);
            });
        });

        describe('when config is not provided', () => {
            it('then control min and max is not displayed', () => {
                scope['vm'].config = null;
                scope.$apply();
                var form: ng.IAugmentedJQuery = element.find(pageObjectSelectors.status);
                expect(form.length).toEqual(0);
            });
        });
    });
}