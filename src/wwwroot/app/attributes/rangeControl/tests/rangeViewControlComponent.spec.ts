/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given range view control', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService;

        var configMock: Common.Models.Dto.IFieldConfig = <Common.Models.Dto.IFieldConfig>{
            active : true,
            required : false
        };

        var unitSchemaMock: Attributes.IRangeControlSchema =
        {
            minControlId : "rent-min",
            maxControlId : "rent-max",
            translationKey: "KEY",
            formName: "formName",
            unit: "GBP"
            };

        var noUnitSchemaMock: Attributes.IRangeControlSchema =
        {
            minControlId : "rent-min",
            maxControlId : "rent-max",
            translationKey : "KEY",
            formName : "formName",
            unit : null
    };

        var pageObjectSelectors = {
            min : '#' + unitSchemaMock.minControlId,
            max: '#' + unitSchemaMock.maxControlId,
            displayValue: '.range-value'
        };

        var setMinMaxValue = (minValue?: number, maxValue?: number) => {
            scope['vm'] = { min: minValue, max: maxValue, config: configMock, schema: noUnitSchemaMock };
            scope.$apply();
        }

        describe('is configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                compile = $compile;
                scope = $rootScope.$new();
                scope['vm'] = { min : 1, max: 2, config : configMock, schema : noUnitSchemaMock };
                element = $compile('<range-view-control min="vm.min" max="vm.max" config="vm.config" schema="vm.schema"></range-view-control>')(scope);
                scope.$apply();
            }));            

            it('then control min and max is displayed', () =>{
                var minElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.min);
                var maxElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.max);
                expect(minElement.length).toBe(1);
                expect(maxElement.length).toBe(1);
            });

            it('when min and max are specified then min and max should be displayed', () => {
                setMinMaxValue(3, 4);

                // \xa0 == &nbsp
                var displayValue = element.find(pageObjectSelectors.displayValue).text();
                expect(displayValue).toBe('3-4');
            });     

            it('when only min is specified then Min word and min value are displayed', () =>{
                setMinMaxValue(3, null);

                var displayValue = element.find(pageObjectSelectors.displayValue).text();
                expect(displayValue).toBe('common.min\xa03');
            });     
            
            it('when only max is specified then Max word and max value are displayed', () => {
                setMinMaxValue(null, 4);

                var displayValue = element.find(pageObjectSelectors.displayValue).text();
                expect(displayValue).toBe('common.max\xa04');
            }); 

            it('when min and max are same then min only should be displayed', () => {
                setMinMaxValue(4, 4);

                var displayValue = element.find(pageObjectSelectors.displayValue).text();
                expect(displayValue).toBe('4');
            }); 
            
            it('when min and max are null then only hyphen should be dispplayed', () => {
                setMinMaxValue(null, null);

                var displayValue = element.find(pageObjectSelectors.displayValue).text();
                expect(displayValue).toBe('-');
            }); 

            it('when unit is specified then should be displayed', () => {
                scope['vm'] = { min: 3, max: 4, config: configMock, schema: unitSchemaMock };
                scope.$apply();

                var displayValue = element.find(pageObjectSelectors.displayValue).text();
                expect(displayValue).toBe('3-4\xa0GBP');
            }); 
                  
        });

        describe('is not configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                scope = $rootScope.$new();
                scope['vm'] = { min: 1, max: 2, schema: unitSchemaMock };
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