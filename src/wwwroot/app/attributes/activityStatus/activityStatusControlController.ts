/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityStatusControlController {
        // TODO set correct status code for type
        private defaultActivityStatusCode: string = 'PreAppraisal';
        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;

        public ngModel: string;

        constructor(private enumService: Services.EnumService) { }

        $onInit = () => {
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: any) => {
            var activityStatuses = result[Dto.EnumTypeCode.ActivityStatus];

            var defaultActivityStatus: any = _.find(activityStatuses, { 'code': this.defaultActivityStatusCode });

            if (defaultActivityStatus) {
                this.ngModel = defaultActivityStatus.id;
            }
        }
    }

    angular.module('app').controller('ActivityStatusControlController', ActivityStatusControlController);
};