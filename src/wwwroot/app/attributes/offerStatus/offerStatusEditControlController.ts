/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;

    export class OfferStatusEditControlController {
        // bindings 
        public ngModel: string;
        public config: IOfferStatusEditControlConfig;
        onOfferStatusChanged: (obj: { offerStatusId: string }) => void;

        // controller
        private allOfferStatuses: Dto.IEnumItem[] = [];

        constructor(private enumService: Services.EnumService) { }

        $onInit = () => {
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        public getOfferStatuses = () => {
            if (!(this.config && this.config.statusId)) {
                return [];
            }

            if (!this.config.statusId.allowedCodes) {
                return this.allOfferStatuses;
            }

            return <Dto.IEnumItem[]>_(this.allOfferStatuses).indexBy('code').at(this.config.statusId.allowedCodes).value();
        }

        public changeOfferStatus = () => {
            this.onOfferStatusChanged({ offerStatusId: this.ngModel });
        }

        private onEnumLoaded = (result: Dto.IEnumDictionary) => {
            this.allOfferStatuses = result[Dto.EnumTypeCode.OfferStatus];
        }
    }

    angular.module('app').controller('OfferStatusEditControlController', OfferStatusEditControlController);
};