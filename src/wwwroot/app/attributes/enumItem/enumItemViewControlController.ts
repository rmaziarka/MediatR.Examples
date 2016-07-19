/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import EnumClassFilter = Antares.Common.Filters.EnumClassFilter;

    export class EnumItemViewControlController {
        // bindings 
        public ngModel: string;
        public config: any;
        public schema: IEnumItemControlSchema;
        public showStatusIcon: boolean;

        //properties
        enumFilter: EnumClassFilter;

        constructor(private enumClassFilter: (model: string) => string) {
        }

        getIconClass = () =>{
            if (!this.showStatusIcon) {
                return '';
            }

            var enumCode = this.enumClassFilter(this.ngModel);
            return "status status-" + enumCode;
        }
    }

    angular.module('app').controller('EnumItemViewControlController', EnumItemViewControlController);
};