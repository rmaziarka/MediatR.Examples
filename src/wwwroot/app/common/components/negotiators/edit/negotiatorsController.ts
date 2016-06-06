/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import DepartmentUserResourceParameters = Common.Models.Resources.IDepartmentUserResourceParameters;

    export class NegotiatorsController {
        public activityId: string;
        public propertyDivisionId: string;
        public departments: Business.ActivityDepartment[];
        public leadNegotiator: Business.ActivityUser;
        public secondaryNegotiators: Business.ActivityUser[];

        public isLeadNegotiatorInEditMode: boolean = false;
        public isSecondaryNegotiatorsInEditMode: boolean = false;

        public negotiatorsSearchOptions: SearchOptions = new SearchOptions();
        public labelTranslationKey: string;
        public nagotiatorCallDateOpened: { [id: string]: boolean; } = {};

        public today: Date = new Date();
        public standardDepartmentType: Dto.IEnumTypeItem;
        public usersSearchMaxCount: number = 100;

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

            var departmentTypes: any = result[Dto.EnumTypeCode.ActivityDepartmentType];
            this.standardDepartmentType = <Dto.IEnumTypeItem>_.find(departmentTypes, { 'code': Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard] });
        }

        public editLeadNegotiator = () => {
            this.isLeadNegotiatorInEditMode = true;
        }

        public changeLeadNegotiator = (user: Dto.IUser) => {
            this.leadNegotiator = this.createActivityUser(user, this.leadNegotiator ? this.leadNegotiator.callDate : null);
            this.fixLeadNegotiatorCallDate();

            this.addDepartment(user.department);

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

            this.addDepartment(user.department);
        }

        public deleteSecondaryNegotiator = (activityUser: Business.ActivityUser) => {
            _.remove(this.secondaryNegotiators, (itm) => itm.userId === activityUser.userId);
        }

        public cancelAddSecondaryNegotiator = () => {
            this.isSecondaryNegotiatorsInEditMode = false;
        }

        public switchToLeadNegotiator = (activityUser: Business.ActivityUser) => {
            var field = this.$scope['negotiatorForm']['callDate'];
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

        public addDepartment(department: Business.Department) {
            if (!_.some(this.departments, { 'departmentId' : department.id })) {
                this.departments.push(this.createActivityDepartment(department));
            }
        }

        public getUsersQuery = (searchValue: string): DepartmentUserResourceParameters => {
            var excludedIds: string[] = _.map<Business.ActivityUser, string>(this.secondaryNegotiators, 'userId');
            excludedIds.push(this.leadNegotiator.userId);

            return { partialName : searchValue, take : this.usersSearchMaxCount, 'excludedIds[]' : excludedIds };
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

        public isSubmitted = (form: any) => {
            while (!!form) {
                if (form.$submitted) return true;
                form = form.$$parentForm;
            }
            return false;
        };

        private fixLeadNegotiatorCallDate = () => {
            if (!this.leadNegotiator.callDate || moment(this.leadNegotiator.callDate).isBefore(this.today, 'day')) {
                this.leadNegotiator.callDate = this.today;
            }
        }

        private createActivityUser = (user: Dto.IUser, callDate: Date) =>{
            var activityUser = new Business.ActivityUser();
            activityUser.activityId = this.activityId;
            activityUser.userId = user.id;
            activityUser.user = new Business.User(<Dto.IUser>user);
            activityUser.user.departmentId = user.department.id;
            activityUser.callDate = callDate;

            return activityUser;
        }

        private createActivityDepartment = (department: Dto.IDepartment) => {
            var activityDepartment = new Business.ActivityDepartment();
            activityDepartment.activityId = this.activityId;
            activityDepartment.departmentId = department.id;
            activityDepartment.department = new Business.Department(<Dto.IDepartment>department);
            activityDepartment.departmentType = this.standardDepartmentType;
            activityDepartment.departmentTypeId = this.standardDepartmentType.id;

            return activityDepartment;
        }
    }

    angular.module('app').controller('NegotiatorsController', NegotiatorsController);
}