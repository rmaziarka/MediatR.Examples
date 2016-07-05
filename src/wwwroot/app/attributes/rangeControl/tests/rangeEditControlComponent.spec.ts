/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given range edit control', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators;

        var configMock: Common.Models.Dto.IFieldConfig = <Common.Models.Dto.IFieldConfig>{
            active : true,
            required : false
        };

        var schemaMock: Attributes.IRangeControlSchema =
        {
            minControlId : "rent-min",
            maxControlId : "rent-max",
            translationKey : "KEY",
            formName : "formName",
            unit : null
    };

        var pageObjectSelectors = {
            min : '#' + schemaMock.minControlId,
            max : '#' + schemaMock.maxControlId
        };

        describe('is configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                scope = $rootScope.$new();
                scope['vm'] = { min : 1, max: 2, config : configMock, schema : schemaMock };
                element = $compile('<range-edit-control min="vm.min" max="vm.max" config="vm.config" schema="vm.schema"></range-edit-control>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));            

            it('then control min and max is displayed', () =>{
                var minElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.min);
                var maxElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.max);
                expect(minElement.length).toBe(1);
                expect(maxElement.length).toBe(1);
            });

            it('when min value is lower than 1 then validation message should be displayed', () => {
                assertValidator.assertMinValueValidator(0, false, pageObjectSelectors.min);
            });

            it('when max value is lower than 1 then validation message should be displayed', () => {
                assertValidator.assertMinValueValidator(0, false, pageObjectSelectors.max);
            });

            it('when min value is greater than max value then validation message should be displayed', () =>{
                var value = 10;
                var max = element.find(pageObjectSelectors.max);
                max.val(value).trigger('input').trigger('change').trigger('blur');
                assertValidator.assertMaxValueValidator(value + 1, false, pageObjectSelectors.min);
            });
        });

        describe('is not configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                scope = $rootScope.$new();
                scope['vm'] = { min: 1, max: 2, schema: schemaMock };
                element = $compile('<range-edit-control min="vm.min" max="vm.max" config="vm.config" schema="vm.schema"></range-edit-control>')(scope);
                scope.$apply();
            }));

            it('then control min and max is not displayed', () => {
                var minElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.min);
                var maxElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.max);
                expect(minElement.length).toBe(0);
                expect(maxElement.length).toBe(0);
            });
        });
    });
}