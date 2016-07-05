/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given percent number view control', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService;

        var configMock: Common.Models.Dto.IFieldConfig = <Common.Models.Dto.IFieldConfig>{
            active : true,
            required: false
        };

        var percentNumberSchema: Attributes.IPercentNumberControlSchema =
        {
            controlId : "mortgage-loan-to-value",
            translationKey : "TRANSLATION.KEY"
        };

        var pageObjectSelectors = {
            controlFieldset : 'fieldset'
        };

        describe('is configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                compile = $compile;
                scope = $rootScope.$new();
                scope['vm'] = { value : 1, config : configMock, schema : percentNumberSchema };
                element = $compile('<percent-number-view-control value="vm.value" config="vm.config" schema="vm.schema"></percent-number-view-control>')(scope);
                scope.$apply();
            }));

            it('then control is displayed', () =>{
                var control: ng.IAugmentedJQuery = element.find(pageObjectSelectors.controlFieldset);
                expect(control.length).toBe(1);
            });

            it('when value is specified then value with percent should be displayed', () =>{
                scope['vm'].value = 1;
                scope.$apply();

                var controlText: string = element.find(pageObjectSelectors.controlFieldset).text();
                expect(controlText).toBe('1%');
            });

            it('when value is not specified then hyphen should be displayed', () =>{
                scope['vm'].value = null;
                scope.$apply();

                var controlText: string = element.find(pageObjectSelectors.controlFieldset).text();
                expect(controlText).toBe('-');
            });
        });

        describe('is not configured', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) =>{

                scope = $rootScope.$new();
                scope['vm'] = { value : 1, schema : percentNumberSchema };
                element = $compile('<percent-number-view-control value="vm.value" config="vm.config" schema="vm.schema"></percent-number-view-control>')(scope);
                scope.$apply();
            }));

            it('then control is not displayed', () =>{
                var control: ng.IAugmentedJQuery = element.find(pageObjectSelectors.controlFieldset);
                expect(control.length).toBe(0);
            });
        });
    });
}