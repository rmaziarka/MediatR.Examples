/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;

    export class EnumSelectController {

        public id: string; // set via binding
        public name: string; // set via binding
        public required: boolean = false; // set via binding
        public ngModel: string; // set via binding
        public enumTypeCode: string; // set via binding
        public hideEmptyValue: boolean; // set via binding
        public selectOption: any;

        private items: Dto.IEnumItem[];

        constructor(private enumService: Services.EnumService) {
            if (this.ngModel) {
                this.items = [<Dto.IEnumItem>{ id: this.ngModel }];
            }

            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }


        private onEnumLoaded = (result: any) => {
            this.items = result[this.enumTypeCode];
        }
    }

    angular.module('app').controller('EnumSelectController', EnumSelectController);
}