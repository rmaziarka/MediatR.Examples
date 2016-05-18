/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
	import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    export class NegotiatorsEditController {
        public leadNegotiator: Business.ActivityUser;
        public secondaryNegotiators: Business.ActivityUser[];

        constructor(
            private dataAccessService: Services.DataAccessService) {
        }

        public changeLeadNegotiator = (user: Dto.IDepartmentUser) => {
            this.leadNegotiator = this.createActivityUser(user, Enums.NegotiatorTypeEnum.LeadNegotiator);
        }

        public cancelChangeLeadSecondaryNegotiator = () => {
            //hide search component
        }

        public addSecondaryNegotiator = (user: Dto.IDepartmentUser) => {
            this.secondaryNegotiators.push(this.createActivityUser(user, Enums.NegotiatorTypeEnum.SecondaryNegotiator));

            //hide search component
        }

        public cancelAddSecondaryNegotiator = () => {
            //hide search component
        }

        public getUsers = (searchValue: string) => {
            return this.dataAccessService
                .getDepartmentUserResource()
                .query({ partialName: searchValue, take: '100', 'excludedIds[]': ['b40e0924-eb18-e611-82a1-80c16efdf78c', 'b40e0924-eb18-e611-82a1-80c16efdf78c'] })
                .$promise
                .then((users: any) => {
                    return users.map((user: Antares.Common.Models.Dto.IDepartmentUser) => { return new Antares.Common.Models.Business.DepartmentUser(<Antares.Common.Models.Dto.IDepartmentUser>user); });
                });
        }

        private createActivityUser = (user: Dto.IDepartmentUser, negotiatorType: Enums.NegotiatorTypeEnum) => {
            return new Business.ActivityUser({
                id: '',
                activityId: null, //this.activity.id,
                userId: user.id,
                user: <Dto.IUser>user,
                userType: negotiatorType
            });
        }
    }

	angular.module('app').controller('NegotiatorsEditController', NegotiatorsEditController);
}