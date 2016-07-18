/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given radio button edit control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery;
            

        var configMock: Common.Models.Dto.IFieldConfig = <Common.Models.Dto.IFieldConfig>{
            active: true,
            required: false
        };

        var schemaMock: Attributes.IRadioButtonsEditControlSchema = {
            formName: "offerContractApprovedControlForm",
            fieldName: "offerContractApproved",
            translationKey: "OFFER.EDIT.CONTRACT_APPROVED",
            radioButtons: [
                { value: true, translationKey: "COMMON.YES" },
                { value: false, translationKey: "COMMON.NO" }]
        }

        var pageObjectSelectors = {
            form: '[name="vm.' + schemaMock.formName + '"]',
            radio: 'input[type="radio"]'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            scope['vm'] = { config: configMock, schema: schemaMock, model: true };
            element = $compile('<radio-buttons-edit-control config="vm.config" schema="vm.schema" ng-model="vm.model"></radio-buttons-edit-control>')(scope);
            scope.$apply();
        }));

        describe('when config is provided', () => {
            it('then form is displayed', () => {
                var form: ng.IAugmentedJQuery = element.find(pageObjectSelectors.form);
                expect(form.length).toEqual(1);
            });

            it('then correct number of radio buttons is displayed', () => {
                var radioButtons = element.find(pageObjectSelectors.radio);
                expect(radioButtons.length).toEqual(schemaMock.radioButtons.length);
            });
        });

        describe('when config is not provided', () => {
            it('then control is not displayed', () => {
                scope['vm'].config = null;
                scope.$apply();
                var form: ng.IAugmentedJQuery = element.find(pageObjectSelectors.form);
                expect(form.length).toEqual(0);
            });
        });
    });
}