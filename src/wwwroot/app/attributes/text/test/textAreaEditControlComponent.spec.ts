/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given text area edit control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators;

        var textAreaModelMock: string = '';
        var configMock: Attributes.ITextControlConfig = TestHelpers.ConfigGenerator.generateTextControlConfig();
        var schemaMock: Attributes.ITextEditControlSchema = {
            controlId: 'text-area-id',
            translationKey: 'textAreaTranslationKey',
            fieldName: 'textArea',
            formName: 'formName'
        };

        var pageObjectSelectors = {
            control: '#' + schemaMock.controlId
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            scope['vm'] = { ngModel: textAreaModelMock, config: configMock, schema: schemaMock };
            element = $compile('<textarea-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></textarea-edit-control>')(scope);
            scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when config is provided', () => {
            it('then control is displayed', () => {
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(1);
            });

            it('and text is longer than 4000 characters then validation is displayed', () =>{
                assertValidator.assertMaxLengthValidator(4001, false, pageObjectSelectors.control);
            });

            it('and text is 4000 characters long then validation is not displayed', () => {
                assertValidator.assertMaxLengthValidator(4000, true, pageObjectSelectors.control);
            });

            describe('when control value is required from config', () => {
                beforeEach(() =>{
                    configMock.textArea.required = true ;
                    scope.$apply();
                });

                it('and text is empty then validation message is displayed', () => {
                    assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.control);
                });

                it('and text is not empty then validation message is not displayed', () => {
                    assertValidator.assertRequiredValidator('test value', true, pageObjectSelectors.control);
                });
            });

            describe('when control value is not required from config', () => {
                beforeEach(() => {
                    configMock.textArea.required = false;
                    scope.$apply();
                });

                it('and text is empty then validation message is not displayed', () => {
                    assertValidator.assertRequiredValidator(null, true, pageObjectSelectors.control);
                });
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