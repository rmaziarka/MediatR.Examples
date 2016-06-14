/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityStatusEditControlController {
        // bindings 
        public ngModel: string;
        public config: IActivityStatusEditControlConfig;
        public onActivityStatusChanged: Function;

        // controller
        public activityStatuses: Dto.IEnumItem[] = [];

        constructor(private enumService: Services.EnumService) { }

        $onInit = () => {
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: Dto.IEnumDictionary) => {
            this.activityStatuses = result[Dto.EnumTypeCode.ActivityStatus];
        }

        changeActivityStatus = () => {
            this.onActivityStatusChanged();
        }
    }

    angular.module('app').controller('ActivityStatusEditControlController', ActivityStatusEditControlController);
};