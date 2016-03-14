/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given contact is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery;

        var pageObjectSelectors = {
            titleSelector: 'input#title',
            firstNameSelector: 'input#first-name',
            surnameSelector: 'input#surname',
            requiredValidatorSelector: '[name="requiredValidationError"]',
            maxLengthValidatorSelector: '[name="maxLengthValidationError"]'
        };

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            element = $compile('<contact-add></contact-add>')(scope);
            scope.$apply();
        }));

        it('when title value is missing then required message should be displayed', () => {
            assertRequiredValidator(null, false, pageObjectSelectors.titleSelector);
        });

        it('when title value is present then required message should not be displayed', () => {
            assertRequiredValidator('Miss', true, pageObjectSelectors.titleSelector);
        });
        
        it('when title value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.titleSelector);
        });

        it('when title value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertMaxLengthValidator(maxLength, true, pageObjectSelectors.titleSelector);
        });

        /////////

        it('when first name value is missing then required message should be displayed', () => {
            assertRequiredValidator(null, false, pageObjectSelectors.firstNameSelector);
        });

        it('when first name value is present then required message should not be displayed', () => {
            assertRequiredValidator('Name', true, pageObjectSelectors.firstNameSelector);
        });

        it('when first name value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.firstNameSelector);
        });

        it('when first name value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertMaxLengthValidator(maxLength, true, pageObjectSelectors.firstNameSelector);
        });

        /////////

        it('when surname value is missing then required message should be displayed', () => {
            assertRequiredValidator(null, false, pageObjectSelectors.surnameSelector);
        });

        it('when surname value is present then required message should not be displayed', () => {
            assertRequiredValidator('Name', true, pageObjectSelectors.surnameSelector);
        });

        it('when surname value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.surnameSelector);
        });

        it('when surname value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertMaxLengthValidator(maxLength, true, pageObjectSelectors.surnameSelector);
        });


        // TODO marta: add faker.js
        function generateString(length: number): string {
            return new Array(length + 1).join('x');
        }

        function assertRequiredValidator(inputValue: string, expectedResult: boolean, inputSelector: string) {
            var input = element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, pageObjectSelectors.requiredValidatorSelector, scope);

            expect(pageObject.isValidFor(inputValue)).toBe(expectedResult);
        }
        
        function assertMaxLengthValidator(inputValueLength: number, expectedResult: boolean, inputSelector: string) {
            var input = element.find(inputSelector);
            
            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, pageObjectSelectors.maxLengthValidatorSelector, scope);

            var value = generateString(inputValueLength);

            expect(pageObject.isValidFor(value)).toBe(expectedResult);
        } 
    });

    // TODO marta: move to other file
    class InputValidationAdapter {
        private pageObject = {
            inputValidCss: 'ng-valid'
        }

        private validationMsg: JQuery;

        constructor(
            private input: ng.IAugmentedJQuery,
            private validatorSelector: string,
            private scope: ng.IScope) {

        }

        public isValidFor(inputValue: string): boolean {
            this.writeValue(inputValue);
            return this.isInputValid() && !this.isValidationShown();
        }

        private isInputValid(): boolean {
            return this.input.hasClass(this.pageObject.inputValidCss);
        }

        private isValidationShown(): boolean {
            return this.input.parent().find(this.validatorSelector).length > 0;
        }

        private writeValue(value: string) {
            this.input.val(value).trigger('input').trigger('blur');
            this.scope.$apply();
        }
    }
}