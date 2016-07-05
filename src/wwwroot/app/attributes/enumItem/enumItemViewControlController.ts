/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class EnumItemViewControlController {
        // bindings 
        public ngModel: string;
        public config: any;
        public schema: IEnumItemControlSchema;
    }

    angular.module('app').controller('EnumItemViewControlController', EnumItemViewControlController);
};