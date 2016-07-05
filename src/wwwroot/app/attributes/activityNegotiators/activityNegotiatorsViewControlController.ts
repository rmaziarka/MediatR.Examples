/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
	import ActivityNegotiatorsControlConfig = Attributes.IActivityNegotiatorsControlConfig;

	export class ActivityNegotiatorsViewControlController {
	    public config: ActivityNegotiatorsControlConfig;

        public activityId: string;
        public propertyDivisionId: string;
        public departments: Business.ActivityDepartment[];
        public leadNegotiator: Business.ActivityUser;
        public canBeEdited: boolean;
        public hideSecondaryNegotiators: boolean = false;
        public secondaryNegotiators: Business.ActivityUser[];

        public labelTranslationKey: string;

        constructor(
            private $scope: ng.IScope,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService) {

            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: any) => {
            var divisions: any = result[Dto.EnumTypeCode.Division];
            var division: any = _.find(divisions, { 'id': this.propertyDivisionId });
            if (division){
                this.labelTranslationKey = division.code.toUpperCase();
            }
        }

        public updateNegotiatorCallDate = (activityUser: Business.ActivityUser) => {
            return (date: Date) => {

                var activityUserToSend: Business.ActivityUser = angular.copy(activityUser);
                activityUserToSend.callDate = date;

                var dto = new Business.UpdateSingleActivityUserResource(activityUserToSend);

                var promise = this.dataAccessService.getActivityUserResource()
                    .update({ id: activityUser.activityId }, dto)
                    .$promise;

                promise.then(() => {
                    activityUser.callDate = moment(date).toDate();
                });

                return promise;
            }
        }
    }

    angular.module('app').controller('ActivityNegotiatorsViewControlController', ActivityNegotiatorsViewControlController);
}