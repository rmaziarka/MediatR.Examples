/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given date range edit control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery;

        var configMock: any = {
            "tenancy_term": {
                term: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            }
        };
        
        var termDateSchema: Antares.Attributes.IDateRangeControlSchema = {
            dateFromControlId: 'termDateFrom',
            dateToControlId: 'termDateTo',
            dateFromTranslationKey: 'TENANCY.COMMON.START_DATE',
            dateToTranslationKey: 'TENANCY.COMMON.END_DATE',
            formName: 'termDateForm',
            fieldName: 'term'
        };

        var pageObjectSelectors = {
            formName: 'ng-form[name="' + termDateSchema.formName + '"]'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            var dateFrom: Date = new Date(2016, 1, 5);
            var dateTo: Date = new Date(2015, 1, 6);
            scope = $rootScope.$new();
            scope['vm'] = { dateFrom: dateFrom, dateTo: dateTo, config: configMock, schema: termDateSchema };
            element = $compile('<date-range-edit-control date-from="vm.dateFrom" date-to="vm.dateTo" config="vm.config" schema="vm.schema"></date-range-edit-control>')(scope);
            scope.$apply();
        }));

        describe('when config is provided', () => {
            it('then control is displayed', () => {
                var control: ng.IAugmentedJQuery = element.find(pageObjectSelectors.formName);
                expect(control.length).toBe(1);
            });
        });

        describe('when config is not provided', () => {
            it('then control is not displayed', () => {
                scope['vm'].config = null;
                scope.$apply();

                var form: ng.IAugmentedJQuery = element.find(pageObjectSelectors.formName);
                expect(form.length).toBe(0);
            });
        });
    });
}