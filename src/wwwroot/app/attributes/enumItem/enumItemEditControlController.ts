/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class EnumItemEditControlController {
        // bindings 
        public ngModel: string;
        public config: any;
        public onSelectedItemChanged: (obj: { id: string }) => void;
        public schema: IEnumItemEditControlSchema;

        // controller
        private allEnumItems: Dto.IEnumItem[] = [];

        constructor(private enumProvider: Antares.Providers.EnumProvider) { }

        $onInit = () => {
            this.allEnumItems = this.enumProvider.enums[this.schema.enumTypeCode];
        }

        public getEnumItems = () => {
            if (!(this.config && this.config[this.schema.fieldName])) {
                return [];
            }

            if (!this.config[this.schema.fieldName].allowedCodes) {
                return this.allEnumItems;
            }

            return <Dto.IEnumItem[]>_(this.allEnumItems).indexBy('code').at(this.config[this.schema.fieldName].allowedCodes).value();
        }

        public changeSelectedItem = () => {
            if (this.onSelectedItemChanged) {
                this.onSelectedItemChanged({ id: this.ngModel });
            }
        }
    }

    angular.module('app').controller('EnumItemEditControlController', EnumItemEditControlController);
};