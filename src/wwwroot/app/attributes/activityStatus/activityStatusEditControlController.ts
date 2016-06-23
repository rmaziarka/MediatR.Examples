/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityStatusEditControlController {
        // bindings 
        public ngModel: string;
        public config: IActivityStatusEditControlConfig;
        onActivityStatusChanged: (obj: { activityStatusId: string }) => void;

        // controller
        private allActivityStatuses: Dto.IEnumItem[] = [];

        constructor(private enumService: Services.EnumService) { }

        $onInit = () => {
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        public getActivityStatuses = () => {
            if (!(this.config && this.config.activityStatusId)) {
                return [];
            }

            if (!this.config.activityStatusId.allowedCodes) {
                return this.allActivityStatuses;
            }

            return <Dto.IEnumItem[]>_(this.allActivityStatuses).indexBy('code').at(this.config.activityStatusId.allowedCodes).value();
        }

        public changeActivityStatus = () => {
            this.onActivityStatusChanged({ activityStatusId: this.ngModel });
        }

        private onEnumLoaded = (result: Dto.IEnumDictionary) => {
            this.allActivityStatuses = result[Dto.EnumTypeCode.ActivityStatus];
        }
    }

    angular.module('app').controller('ActivityStatusEditControlController', ActivityStatusEditControlController);
};