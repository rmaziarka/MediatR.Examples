/// <reference path="../../../typings/_all.d.ts" />

module Antares {

    export module Common {

        export module Filters {

            export class EnumSorterFilter {
                constructor(private enumProvider: Antares.Providers.EnumProvider, public appConfig: Common.Models.IAppConfig) {
                }

                getEnumValue = (enumItemId: string) => {
                    return this.enumProvider.getEnumCodeById(enumItemId);
                }

                sort = (items: any[], enumType: string, field: string): any => {
                    if (!!items === false) {
                        return items;
                    }
                    var filtered: any[] = items.slice();

                    filtered.sort((nextOffer: any, previousOffer: any) => {
                        var nextOfferEnumValue = this.getEnumValue(nextOffer[field]);
                        var previousOfferEnumValue = this.getEnumValue(previousOffer[field]);
                        return this.appConfig.enumOrder[enumType][nextOfferEnumValue] - this.appConfig.enumOrder[enumType][previousOfferEnumValue];
                    });

                    return filtered;
                };

                static getInstance(enumProvider: Antares.Providers.EnumProvider, appConfig: Common.Models.IAppConfig) {
                    var filterFunc: any = (items: any, enumType: string, field: string) => {
                        var filter = new EnumSorterFilter(enumProvider, appConfig);
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