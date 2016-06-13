/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityStatusEditControlController {
        // bindings 
        public ngModel: string;

        // TODO set correct status code for type
        private defaultActivityStatusCode: string = 'PreAppraisal';

        public activityStatuses: Dto.IEnumItem[] = [];
        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;
        public selectedStatus: Dto.IEnumItem = null;

        constructor(private enumService: Services.EnumService) { }

        $onInit = () => {
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: Dto.IEnumDictionary) => {
            this.activityStatuses = result[Dto.EnumTypeCode.ActivityStatus];

            var defaultActivityStatus: any = _.find(this.activityStatuses, { 'code': this.defaultActivityStatusCode });

            if (defaultActivityStatus) {
                this.ngModel = defaultActivityStatus.id;
                this.selectedStatus = defaultActivityStatus;
            }
        }

        changeStatus = () => {
            this.ngModel = this.selectedStatus ? this.selectedStatus.id : null;
        }
    }

    angular.module('app').controller('ActivityStatusEditControlController', ActivityStatusEditControlController);
};