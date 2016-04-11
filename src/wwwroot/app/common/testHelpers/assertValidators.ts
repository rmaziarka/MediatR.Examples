/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Resources = Antares.Common.Models.Resources;

    export class AssertValidators {
        private pageObjectSelectors = {
            requiredValidatorSelector : '[name="requiredValidationError"]',
            minNumberValidationError : '[name="minNumberValidationError"]',
            maxNumberValidationError : '[name="maxNumberValidationError"]',
            maxLengthValidatorSelector: '[name="maxLengthValidationError"]',
            formatValidationError: '[name="formatValidationError"]'
        };

        constructor(private element: ng.IAugmentedJQuery, private scope: ng.IScope){
            this.element = element;
            this.scope = scope;
        }

        public assertRequiredValidator = (inputValue: string, expectedResult: boolean, inputSelector: string) =>{
            var input = this.element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, this.pageObjectSelectors.requiredValidatorSelector, this.scope);

            expect(pageObject.isValidFor(inputValue)).toBe(expectedResult);
        }

        public assertMinValueValidator = (inputMinValue: number, expectedResult: boolean, inputSelector: string) =>{
            var input = this.element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, this.pageObjectSelectors.minNumberValidationError, this.scope);

            expect(pageObject.isValidFor(inputMinValue)).toBe(expectedResult);
        }

        public assertMaxValueValidator = (inputMaxValue: number, expectedResult: boolean, inputSelector: string) =>{
            var input = this.element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, this.pageObjectSelectors.maxNumberValidationError, this.scope);

            expect(pageObject.isValidFor(inputMaxValue)).toBe(expectedResult);
        }

        public assertMaxLengthValidator = (inputValueLength: number, expectedResult: boolean, inputSelector: string) =>{
            var input = this.element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, this.pageObjectSelectors.maxLengthValidatorSelector, this.scope);

            var value = this.generateString(inputValueLength);

            expect(pageObject.isValidFor(value)).toBe(expectedResult);
        }

        public assertPatternValidator = (inputValue: string, expectedResult: boolean, inputSelector: string) => {
            var input = this.element.find(inputSelector);

            var pageObject: InputValidationAdapter =
                new InputValidationAdapter(input, this.pageObjectSelectors.formatValidationError, this.scope);

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

    class InputValidationAdapter {
        private pageObject = {
            inputValidCss : 'ng-valid'
        }

        private validationMsg: JQuery;

        constructor(
            private input: ng.IAugmentedJQuery,
            private validatorSelector: string,
            private scope: ng.IScope){

        }

        public isValidFor(inputValue: any): boolean{
            this.writeValue(inputValue);
            return this.isInputValid() && !this.isValidationShown();
        }

        private isInputValid(): boolean{
            return this.input.hasClass(this.pageObject.inputValidCss);
        }

        private isValidationShown(): boolean{
            return this.input.parent().find(this.validatorSelector).length > 0;
        }

        private writeValue(value: string){
            this.input.val(value).trigger('input').trigger('change').trigger('blur');
            this.scope.$apply();
        }
    }

    angular.module('app').service('assertValidators', AssertValidators);
}