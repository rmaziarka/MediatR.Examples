/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import DepartmentUserResourceParameters = Common.Models.Resources.IDepartmentUserResourceParameters;
    import SearchOptions = Antares.Common.Component.SearchOptions;

	export interface IActivityNegotiatorsEditControlForm extends ng.IFormController {
		callDate: ng.INgModelController
	}

    export class ActivityNegotiatorsEditControlController {
        // bindings
        onNegotiatorAdded: (obj: { user: Dto.IUser }) => void;
        onNegotiatorRemoved: () => void;
        config: IActivityDepartmentsViewControlConfig;

        public activityId: string;
        public propertyDivisionId: string;
        public leadNegotiator: Business.ActivityUser;
        public secondaryNegotiators: Business.ActivityUser[];

        public isLeadNegotiatorInEditMode: boolean = false;
        public isSecondaryNegotiatorsInEditMode: boolean = false;

        public negotiatorsSearchOptions: SearchOptions = new SearchOptions();
        public labelTranslationKey: string;
        public nagotiatorCallDateOpened: { [id: string]: boolean; } = {};

        public today: Date = new Date();
        public usersSearchMaxCount: number = 100;

		public negotiatorsForm: IActivityNegotiatorsEditControlForm;

        constructor(
            private $scope: ng.IScope,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService) {

            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: any) => {
            var divisions: any = result[Dto.EnumTypeCode.Division];
            var division: any = _.find(divisions, { 'id': this.propertyDivisionId });
            if (division) {
                this.labelTranslationKey = division.code.toUpperCase();
            }
        }

        public editLeadNegotiator = () => {
            this.isLeadNegotiatorInEditMode = true;
        }

        public changeLeadNegotiator = (user: Dto.IUser) => {
            this.leadNegotiator = this.createActivityUser(user, this.leadNegotiator ? this.leadNegotiator.callDate : null);
            this.fixLeadNegotiatorCallDate();

            this.onNegotiatorAdded({ user: user });

            this.isLeadNegotiatorInEditMode = false;
        }

        public cancelChangeLeadNegotiator = () => {
            this.isLeadNegotiatorInEditMode = false;
        }

        public editSecondaryNegotiators = () => {
            this.isSecondaryNegotiatorsInEditMode = true;
        }

        public addSecondaryNegotiator = (user: Dto.IUser) => {
            this.secondaryNegotiators.push(this.createActivityUser(user, null));

            this.onNegotiatorAdded({ user: user });
        }

        public deleteSecondaryNegotiator = (activityUser: Business.ActivityUser) => {
            _.remove(this.secondaryNegotiators, (itm) => itm.userId === activityUser.userId);

            this.onNegotiatorRemoved();
        }

        public cancelAddSecondaryNegotiator = () => {
            this.isSecondaryNegotiatorsInEditMode = false;
        }

        public switchToLeadNegotiator = (activityUser: Business.ActivityUser) => {
            var field = this.negotiatorsForm.callDate;
            if (field.$invalid && field.$dirty) {
                this.leadNegotiator.callDate = null;
            }

            _.remove(this.secondaryNegotiators, (itm) => itm.userId === activityUser.userId);
            this.secondaryNegotiators.push(this.leadNegotiator);

            activityUser.callDate = activityUser.callDate || this.leadNegotiator.callDate;
            this.leadNegotiator = activityUser;
            this.fixLeadNegotiatorCallDate();
        }

        public openNegotiatorCallDate = (negotiatorUserId: string) => {
            this.nagotiatorCallDateOpened[negotiatorUserId] = true;
        }

        public getUsersQuery = (searchValue: string): DepartmentUserResourceParameters => {
            var excludedIds: string[] = _.map<Business.ActivityUser, string>(this.secondaryNegotiators, 'userId');
            excludedIds.push(this.leadNegotiator.userId);

            return { partialName: searchValue, take: this.usersSearchMaxCount, 'excludedIds[]': excludedIds };
        }

        public getUsers = (searchValue: string) => {
            var query = this.getUsersQuery(searchValue);

            return this.dataAccessService
                .getDepartmentUserResource()
                .query(query)
                .$promise
                .then((users: any) => {
                    return users.map((user: Common.Models.Dto.IUser) => { return new Common.Models.Business.User(<Common.Models.Dto.IUser>user); });
                });
        }

        private fixLeadNegotiatorCallDate = () => {
            if (!this.leadNegotiator.callDate || moment(this.leadNegotiator.callDate).isBefore(this.today, 'day')) {
                this.leadNegotiator.callDate = this.today;
            }
        }

        private createActivityUser = (user: Dto.IUser, callDate: Date) => {
            var activityUser = new Business.ActivityUser();
            activityUser.activityId = this.activityId;
            activityUser.userId = user.id;
            activityUser.user = new Business.User(<Dto.IUser>user);
            activityUser.user.departmentId = user.department.id;
            activityUser.callDate = callDate;

            return activityUser;
        }
    }

    angular.module('app').controller('ActivityNegotiatorsEditControlController', ActivityNegotiatorsEditControlController);
}