/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given text view control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery;

        var textModelMock: any = {};
        var configMock: any = {
            active: true,
            text: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
        };
        var schemaMock: Attributes.ITextControlSchema = {
            controlId: 'text-id',
            translationKey: 'textTranslationKey'
        };

        var pageObjectSelectors = {
            control: '#' + schemaMock.controlId
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            scope['vm'] = { ngModel: textModelMock, config: configMock, schema: schemaMock };
            element = $compile('<text-view-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></text-view-control>')(scope);
            scope.$apply();
        }));

        describe('when config is provided', () => {
            it('then control is displayed', () => {
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(1);
            });

        });

        describe('when config is not provided', () => {
            it('then control is not displayed', () => {
                scope['vm'].config = null;
                scope.$apply();
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
        });
    });
}