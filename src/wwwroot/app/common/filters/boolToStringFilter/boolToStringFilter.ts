/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Filters {
    const translationYes: string = "COMMON.YES";
    const translationNo: string = "COMMON.NO";
    
    // this is a first version - if needed, can be extended to use custom translation keys
    export class BoolToStringFilter {
        
        constructor(private $translate: ng.translate.ITranslateService) {
        }

        transform = (value?: boolean): any =>{
            let canBeTranslated = (value !== null && value !== undefined && typeof value === "boolean");

            if (canBeTranslated) {
                let translationKey = (value ? translationYes : translationNo);
                return this.$translate.instant(translationKey);
            }
            else {
                return "";
            }
        };

        static getInstance($translate: ng.translate.ITranslateService) {
            return (value?: boolean) => {
                var filter = new BoolToStringFilter($translate);
                return filter.transform(value);
            };
        }
    }

    angular.module('app').filter('boolToString', BoolToStringFilter.getInstance);
}