/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    export class AssertValidators {
        private pageObjectSelectors = {
            requiredValidatorSelector : '[name="requiredValidationError"]',
            minNumberValidationError: '[name="minNumberValidationError"]',
            maxNumberValidationError: '[name="maxNumberValidationError"]',
            numberGreaterThanValidationError: '[name="numberGreaterThanValidationError"]',
            maxLengthValidatorSelector: '[name="maxLengthValidationError"], [name="kfMaxCountValidationError"]',
            formatValidationError: '[name="formatValidationError"]',
            dateFormatValidationError: '[name="dateValidationError"]',
            numberFormatValidationError: '[name="numberValidationError"]',
            anyValidationMessageError: 'ng-message'
        };

        constructor(private element: ng.IAugmentedJQuery, private scope: ng.IScope){
            this.element = element;
            this.scope = scope;
        }

        public assertRequiredValidator = (inputValue: string | number, expectedResult: boolean, inputSelector: string, parentSelector?: string) => {
            this.assertValidator(inputValue, expectedResult, inputSelector, this.pageObjectSelectors.requiredValidatorSelector, parentSelector);
        }

        public assertMinValueValidator = (inputMinValue: number, expectedResult: boolean, inputSelector: string, parentSelector?: string) => {
            this.assertValidator(inputMinValue, expectedResult, inputSelector, this.pageObjectSelectors.minNumberValidationError, parentSelector);
        }

        public assertMaxValueValidator = (inputMaxValue: number, expectedResult: boolean, inputSelector: string, parentSelector?: string) => {
            this.assertValidator(inputMaxValue, expectedResult, inputSelector, this.pageObjectSelectors.maxNumberValidationError, parentSelector);
        }

        public assertMaxLengthValidator = (inputValueLength: number, expectedResult: boolean, inputSelector: string, parentSelector?: string) => {
            var value = this.generateString(inputValueLength);

            this.assertValidator(value, expectedResult, inputSelector, this.pageObjectSelectors.maxLengthValidatorSelector, parentSelector);
        }

        public assertNumberGreaterThenValidator = (inputMaxValue: number, expectedResult: boolean, inputSelector: string, parentSelector?: string) => {
            this.assertValidator(inputMaxValue, expectedResult, inputSelector, this.pageObjectSelectors.numberGreaterThanValidationError, parentSelector);
        }

        public assertPatternValidator = (inputValue: string, expectedResult: boolean, inputSelector: string, parentSelector?: string) => {
            this.assertValidator(inputValue, expectedResult, inputSelector, this.pageObjectSelectors.formatValidationError, parentSelector);
        }

        public assertDateFormatValidator = (inputValue: string, expectedResult: boolean, inputSelector: string, parentSelector?: string) => {
            this.assertValidator(inputValue, expectedResult, inputSelector, this.pageObjectSelectors.dateFormatValidationError, parentSelector);
        }

        public assertNumberFormatValidator = (inputValue: string, expectedResult: boolean, inputSelector: string, parentSelector?: string) =>{
            this.assertValidator(inputValue, expectedResult, inputSelector, this.pageObjectSelectors.numberFormatValidationError, parentSelector);
        }

        public assertValidAndNoMessages = (inputValue: string, inputSelector: string, parentSelector?: string) =>{
            var input = this.element.find(inputSelector);
            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, this.pageObjectSelectors.anyValidationMessageError, this.scope, parentSelector);

            pageObject.writeValue(inputValue);
            expect(pageObject.isInputValid()).toBe(true, 'Input is invalid');
            expect(pageObject.isValidationShown()).toBe(false, 'At least one error message is visible');
        }

        private assertValidator = (inputValue: any, expectedResult: boolean, inputSelector: string, errorSelector: string, parentSelector?: string) => {
            var input = this.element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, errorSelector, this.scope, parentSelector);

            pageObject.writeValue(inputValue);

            var isValidMessage = expectedResult ? "Input is invalid but is expected to be valid" : "Input is valid but is expected to be invalid";
            var isValidationShownMessage = expectedResult ? "Validation message is shown but input is valid" : "Validation message is not shown or too many validation messages shown";

            expect(pageObject.isInputValid()).toBe(expectedResult, isValidMessage);
            expect(pageObject.isValidationShown()).toBe(!expectedResult, isValidationShownMessage);
        }

        // TODO: method name not valid for what it does... use everywhere method below (assertElementHasHideClass)
        public assertShowElement = (expectedResult: boolean, elementSelector: string) => {
            var selectedElement = this.element.find(elementSelector);
            expect(selectedElement.hasClass("ng-hide")).toBe(expectedResult);
        }
        
        public assertElementHasHideClass = (expectedResult: boolean, elementSelector: string) => {
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
            private scope: ng.IScope,
            private parentSelector: string = '.form-group') {

        }

        public isInputValid(): boolean {
            return this.input.hasClass(this.pageObject.inputValidCss);
        }

        public isValidationShown(): boolean{
            return this.input.closest(this.parentSelector).find(this.validatorSelector).length === 1;
        }

        public writeValue(value: string){
            this.input.val(value).trigger('input').trigger('change').trigger('blur');
            this.scope.$apply();
        }
    }

    angular.module('app').service('assertValidators', AssertValidators);
}