/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    export class CheckboxListEditControlController {
        // bindings 
        public ngModel: any[];
        public config: any;
        public schema: Attributes.ICheckboxListEditControlSchema;

        constructor() {
            this.ngModel = this.ngModel || [];
            _.each(this.schema.checkboxes, (checkbox: Attributes.ICheckboxSchema) => { checkbox.selected = this.isSelected(checkbox); });
        }

        private isSelected = (checkbox: Attributes.ICheckboxSchema) => {
            return this.ngModel.indexOf(checkbox.value) > -1;
        }

        changeSelection = (checkbox: Attributes.ICheckboxSchema) => {
            var indexOfValue = this.ngModel.indexOf(checkbox.value);
            if (indexOfValue > -1) {
                // is currently selected
                this.ngModel.splice(indexOfValue, 1);
            }
            else {
                // is newly selected
                this.ngModel.push(checkbox.value);
            }
        }
    }

    angular.module('app').controller('CheckboxListEditControlController', CheckboxListEditControlController);
}