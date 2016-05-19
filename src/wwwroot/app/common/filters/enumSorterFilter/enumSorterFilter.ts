/// <reference path="../../../typings/_all.d.ts" />

module Antares {

    export module Common {

        export module Filters {

            export class EnumSorterFilter {
                private filter: any;

                constructor(public $filter: ng.IFilterService, public appConfig: Common.Models.IAppConfig) {
                    this.filter = this.$filter('dynamicTranslate');
                }

                sort = (items: any, enumType: string, field: string): any => {
                    var filtered: any[] = [];
                    angular.forEach(items, (item) => {
                        filtered.push(item);
                    });

                    filtered.sort((nextOffer: any, previousOffer: any) => {
                        var nextOfferEnumValue = this.filter(nextOffer[field]);
                        var previousOfferEnumValue = this.filter(previousOffer[field]);
                        return this.appConfig.enumOrder[enumType][nextOfferEnumValue] > this.appConfig.enumOrder[enumType][previousOfferEnumValue] ? 1 : -1;
                    });

                    return filtered;
                };

                static getInstance($filter: ng.IFilterService, appConfig: Common.Models.IAppConfig) {
                    var filterFunc: any = (items: any, enumType: string, field: string) => {
                        var filter = new EnumSorterFilter($filter, appConfig);
                        return filter.sort(items, enumType, field);
                    };
                    filterFunc.$stateful = true;

                    return filterFunc;
                };
            }

            angular.module('app').filter('enumSorter', EnumSorterFilter.getInstance);
        }
    }
}