/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class EnumItemEditControlController {
        // bindings 
        public ngModel: string;
        public config: IEnumSelectEditControlConfig;
        public onSelectedItemChanged: (obj: { id: string }) => void;
        public enumTypeCode: Dto.EnumTypeCode;
        public labelKey: string;

        // controller
        private allEnumItems: Dto.IEnumItem[] = [];

        constructor(private enumProvider: Antares.Providers.EnumProvider) { }

        $onInit = () => {
            this.allEnumItems = this.enumProvider.enums[this.enumTypeCode];
        }

        public getEnumItems = () => {
            if (!(this.config && this.config.enumItem)) {
                return [];
            }

            if (!this.config.enumItem.allowedCodes) {
                return this.allEnumItems;
            }

            return <Dto.IEnumItem[]>_(this.allEnumItems).indexBy('code').at(this.config.enumItem.allowedCodes).value();
        }

        public changeSelectedItem = () => {
            this.onSelectedItemChanged({ id: this.ngModel });
        }
    }

    angular.module('app').controller('EnumItemEditControlController', EnumItemEditControlController);
};