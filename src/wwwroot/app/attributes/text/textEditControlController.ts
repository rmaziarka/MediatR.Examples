/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class TextEditControlController {
        // bindings 
        public ngModel: string;
        public config: any;
        public schema: ITextEditControlSchema;
    }

    angular.module('app').controller('TextEditControlController', TextEditControlController);
};