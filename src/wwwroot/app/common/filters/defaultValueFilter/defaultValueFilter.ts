/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Filters {
    export class DefaultValueFilter {
        transform = (value: any): any =>{
            return value || '-';
        };

        static getInstance() {
            return (value: any) => {
                var filter = new DefaultValueFilter();
                return filter.transform(value);
            };
        }
    }

    angular.module('app').filter('defaultValue', DefaultValueFilter.getInstance);
}