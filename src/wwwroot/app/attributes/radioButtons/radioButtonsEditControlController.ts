/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    export class RadioButtonsEditControlController {
        // bindings 
        public ngModel: string;
        public config: any;
        public onSelectedItemChanged: (obj: { value: string }) => void;
        public schema: IRadioButtonsEditControlSchema;

        public changeSelectedItem = () => {
            if (this.onSelectedItemChanged) {
                this.onSelectedItemChanged({ value: this.ngModel });
            }
        }
    }

    angular.module('app').controller('RadioButtonsEditControlController', RadioButtonsEditControlController);
};