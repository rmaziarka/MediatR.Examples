/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
	import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    export class NegotiatorsEditController {
        public activityId: string;
        public leadNegotiator: Business.ActivityUser;
        public secondaryNegotiators: Business.ActivityUser[];

        private usersSearchMaxCount: number = 100;

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

        public getUsers = (searchValue: string) =>{
            var excludedIds: string[] = _.map<Business.ActivityUser, string>(this.secondaryNegotiators, 'userId');
            excludedIds.push(this.leadNegotiator.userId);

            return this.dataAccessService
                .getDepartmentUserResource()
                .query({ partialName: searchValue, take: this.usersSearchMaxCount, 'excludedIds[]': excludedIds })
                .$promise
                .then((users: any) => {
                    return users.map((user: Common.Models.Dto.IDepartmentUser) => { return new Common.Models.Business.DepartmentUser(<Common.Models.Dto.IDepartmentUser>user); });
                });
        }

        private createActivityUser = (user: Dto.IDepartmentUser, negotiatorType: Enums.NegotiatorTypeEnum) => {
            return new Business.ActivityUser({
                id: '',
                activityId: this.activityId,
                userId: user.id,
                user: <Dto.IUser>user,
                userType: negotiatorType
            });
        }
    }

	angular.module('app').controller('NegotiatorsEditController', NegotiatorsEditController);
}