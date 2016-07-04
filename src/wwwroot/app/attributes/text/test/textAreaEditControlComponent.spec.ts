/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityAskingPriceControlConfig = Antares.Attributes.IActivityAskingPriceControlConfig;
    describe('Given text area edit control', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators;

        var textAreaMock: number = 1;
        var configMock: ActivityAskingPriceControlConfig = TestHelpers.ConfigGenerator.generateActivityAskingPriceConfig();
        var schemaMock: Attributes.ITextEditControlSchema = {
            controlId: 'text-area-id',
            translationKey: 'textAreaTranslationKey',
            fieldName: 'textArea',
            formName: 'formName'
        };

        var pageObjectSelectors = {
            control: schemaMock.controlId,
            text: '#' + schemaMock.controlId
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            scope['vm'] = { ngModel: textAreaMock, config: configMock, schema: schemaMock };
            element = $compile('<textarea-edit-control ng-model="vm.ngModel" config="vm.config" schema="vm.schema"></textarea-edit-control>')(scope);
            scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('and config is provided', () => {
            it('then text is displayed', () => {
                var textElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.text);
                expect(textElement.length).toBe(1);
            });

            it('when text is longer than 4000 characters then validation should be displayed', () =>{
                assertValidator.assertMaxLengthValidator(4001, false, pageObjectSelectors.text);
            });

            it('when text is 4000 characters long then validation should not be displayed', () => {
                assertValidator.assertMaxLengthValidator(4000, true, pageObjectSelectors.text);
            });

            describe('when text value is required from config', () => {
                beforeEach(() =>{
                    configMock['textArea'] = { required: true };
                    scope.$apply();
                });

                it('and text is empty then validation message should be displayed', () => {
                    assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.text);
                });

                it('and text is not empty then validation message should not be displayed', () => {
                    assertValidator.assertRequiredValidator('test value', true, pageObjectSelectors.text);
                });
            });

            describe('when text value is not required from config', () => {
                beforeEach(() => {
                    configMock['textArea'] = { required: false };
                    scope.$apply();
                });

                it('and text is empty then validation message should not be displayed', () => {
                    assertValidator.assertRequiredValidator(null, true, pageObjectSelectors.text);
                });
            });
        });

        describe('and config is not provided', () => {
            it('then control is not displayed', () => {
                scope['vm'].config = null;
                scope.$apply();
                var controlElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.control);
                expect(controlElement.length).toBe(0);
            });
        });
    });
}