/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    export class AssertValidators {
        private pageObjectSelectors = {
            requiredValidatorSelector : '[name="requiredValidationError"]',
            minNumberValidationError : '[name="minNumberValidationError"]',
            maxNumberValidationError: '[name="maxNumberValidationError"]',
            numberGreaterThanValidationError: '[name="numberGreaterThanValidationError"]',
            maxLengthValidatorSelector: '[name="maxLengthValidationError"], [name="kfMaxCountValidationError"]',
            formatValidationError: '[name="formatValidationError"]'
        };

        constructor(private element: ng.IAugmentedJQuery, private scope: ng.IScope){
            this.element = element;
            this.scope = scope;
        }

        public assertRequiredValidator = (inputValue: string | number, expectedResult: boolean, inputSelector: string) => {
            this.assertValidator(inputValue, expectedResult, inputSelector, this.pageObjectSelectors.requiredValidatorSelector);
        }

        public assertMinValueValidator = (inputMinValue: number, expectedResult: boolean, inputSelector: string) => {
            this.assertValidator(inputMinValue, expectedResult, inputSelector, this.pageObjectSelectors.minNumberValidationError);
        }

        public assertMaxValueValidator = (inputMaxValue: number, expectedResult: boolean, inputSelector: string) => {
            this.assertValidator(inputMaxValue, expectedResult, inputSelector, this.pageObjectSelectors.maxNumberValidationError);
        }

        public assertMaxLengthValidator = (inputValueLength: number, expectedResult: boolean, inputSelector: string) => {
            var value = this.generateString(inputValueLength);

            this.assertValidator(value, expectedResult, inputSelector, this.pageObjectSelectors.maxLengthValidatorSelector);
        }

        public assertNumberGreaterThenValidator = (inputMaxValue: number, expectedResult: boolean, inputSelector: string) => {
            this.assertValidator(inputMaxValue, expectedResult, inputSelector, this.pageObjectSelectors.numberGreaterThanValidationError);
        }

        public assertPatternValidator = (inputValue: string, expectedResult: boolean, inputSelector: string) => {
            this.assertValidator(inputValue, expectedResult, inputSelector, this.pageObjectSelectors.formatValidationError);
        }

        private assertValidator = (inputValue: any, expectedResult: boolean, inputSelector: string, errorSelector: string) => {
            var input = this.element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, errorSelector, this.scope);

            expect(pageObject.isValidFor(inputValue)).toBe(expectedResult);
        }

        public assertShowElement = (expectedResult: boolean, elementSelector: string) => {
            var selectedElement = this.element.find(elementSelector);
            expect(selectedElement.hasClass("ng-hide")).toBe(expectedResult);
        }

        private generateString = (length: number): string =>{
            return new Array(length + 1).join('x');
        }
    }

   export class InputValidationAdapter {
        private pageObject = {
            inputValidCss : 'ng-valid'
        }

        private validationMsg: JQuery;

        constructor(
            private input: ng.IAugmentedJQuery,
            private validatorSelector: string,
            private scope: ng.IScope){

        }

        public isValidFor(inputValue: any): boolean {
            this.writeValue(inputValue);
            return this.isInputValid() && !this.isValidationShown();
        }

        public isInputValid(): boolean {
            return this.input.hasClass(this.pageObject.inputValidCss);
        }

        public isValidationShown(): boolean{
            return this.input.parents('.form-group').find(this.validatorSelector).length > 0;
        }

        public writeValue(value: string){
            this.input.val(value).trigger('input').trigger('change').trigger('blur');
            this.scope.$apply();
        }
    }

    angular.module('app').service('assertValidators', AssertValidators);
}