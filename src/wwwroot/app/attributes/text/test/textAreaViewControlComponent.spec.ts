/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given text area view control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators;

        var textAreaModelMock: any = {};
        var configMock: Attributes.ITextControlConfig = TestHelpers.ConfigGenerator.generateTextControlConfig();
        var schemaMock: Attributes.ITextControlSchema = {
            controlId: 'text-area-id',
            translationKey: 'textAreaTranslationKey'
        };

        var pageObjectSelectors = {
            control: '#' + schemaMock.controlId
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            scope['vm'] = { ngModel: textAreaModelMock, config: configMock, schema: schemaMock };
            element = $compile('<textarea-view-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></textarea-view-control>')(scope);
            scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
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