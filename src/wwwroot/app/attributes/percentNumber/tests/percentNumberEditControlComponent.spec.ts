/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given percent number edit control', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators;

        var configMock: any = <Common.Models.Dto.IFieldConfig>{
            active : true,
            required : false,
            percentNumber : TestHelpers.ConfigGenerator.generateFieldConfig()
        };

        var schemaMock: Attributes.IPercentNumberEditControlSchema =
        {
            controlId : "mortgage-loan-to-value",
            translationKey : "TRANSLATION.KEY",
            formName: "testForm",
            fieldName: "percentNumber"
        };

        var pageObjectSelectors = {
            control : '#' + schemaMock.controlId,
        };

        describe('is configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                scope = $rootScope.$new();
                scope['vm'] = { value : 1, config : configMock, schema : schemaMock };
                element = $compile('<percent-number-edit-control value="vm.value" config="vm.config" schema="vm.schema"></percent-number-edit-control>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('then control is displayed', () =>{
                var control: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(control.length).toBe(1);
            });

            it('when value is lower than 0 then validation message should be displayed', () =>{
                assertValidator.assertMinValueValidator(-1, false, pageObjectSelectors.control);
            });

            it('when maxValue is not specified and value is higher than default 100 then validation message should be displayed', () =>{
                scope['vm'].maxValue = null;
                assertValidator.assertMaxValueValidator(101, false, pageObjectSelectors.control);
            });

            it('when maxValue is specified and value is higher than maxValue then validation message should be displayed', () =>{
                scope['vm'].maxValue = 2000;
                assertValidator.assertMaxValueValidator(2001, false, pageObjectSelectors.control);
            });

            it('and text field is required form config then validation message is displayed', () =>{
                configMock.percentNumber.required = true;
                scope.$apply();
                assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.control);
            });
        });

        describe('is not configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                scope = $rootScope.$new();
                scope['vm'] = { value : 1, schema : schemaMock };
                element = $compile('<percent-number-edit-control value="vm.min" config="vm.config" schema="vm.schema"></percent-number-edit-control>')(scope);
                scope.$apply();
            }));

            it('then control is not displayed', () =>{
                var control: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(control.length).toBe(0);
            });
        });
    });
}