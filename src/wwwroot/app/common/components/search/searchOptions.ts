/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class SearchOptions {
        isRequired = false;
        isEditable = false;
        nullOnSelect = true;
        showCancelButton = true;
        maxLength = null;
        minLength = 3;
        modelOptions = {
            debounce: {
                default: 250,
                blur: 250
            }
        };

        constructor(specificOptions?: any) {
            if (specificOptions) {
                angular.extend(this, specificOptions);
            }
        }
    }
}