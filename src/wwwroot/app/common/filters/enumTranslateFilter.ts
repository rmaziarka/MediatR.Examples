/// <reference path="../../typings/_all.d.ts" />

module Antares {

    export module Common {

        export module Filters {

            export class EnumTranslateFilter {

                private TRANSLATE_ENUMS_PREFIX = "ENUMS.";

                constructor(private $translate: ng.translate.ITranslateService) {
                }

                translate = (value: String): String => {
                    return this.$translate.instant(this.TRANSLATE_ENUMS_PREFIX + value);
                };

                static getInstance($translate: ng.translate.ITranslateService) {
                    var translateFunc: any;
                    translateFunc = (value: string) => {
                        var filter = new EnumTranslateFilter($translate);
                        return filter.translate(value);
                    };
                    translateFunc.$stateful = true;

                    return translateFunc;
                }
            }

            angular.module('app').filter('enumTranslate', EnumTranslateFilter.getInstance);
        }
    }
}