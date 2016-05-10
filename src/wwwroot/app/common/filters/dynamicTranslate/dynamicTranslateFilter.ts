/// <reference path="../../../typings/_all.d.ts" />

module Antares {

    export module Common {

        export module Filters {

            export class DynamicTranslateFilter {

                public static DYNAMIC_TRANSLATIONS_KEY = 'DYNAMICTRANSLATIONS';

                constructor(private $translate: ng.translate.ITranslateService) {
                }

                translate = (value: String): String => {
                    return this.$translate.instant(DynamicTranslateFilter.DYNAMIC_TRANSLATIONS_KEY + '.' + value);
                };

                static getInstance($translate: ng.translate.ITranslateService) {
                    var translateFunc: any;
                    translateFunc = (value: string) => {
                        var filter = new DynamicTranslateFilter($translate);
                        return filter.translate(value);
                    };
                    translateFunc.$stateful = true;

                    return translateFunc;
                }
            }

            angular.module('app').filter('dynamicTranslate', DynamicTranslateFilter.getInstance);
        }
    }
}