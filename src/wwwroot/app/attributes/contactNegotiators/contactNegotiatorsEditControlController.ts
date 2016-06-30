/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import DepartmentUserResourceParameters = Common.Models.Resources.IDepartmentUserResourceParameters;
    import SearchOptions = Antares.Common.Component.SearchOptions;

	export interface IContactNegotiatorsEditControlForm extends ng.IFormController {
		callDate: ng.INgModelController
	}

    export class ContactNegotiatorsEditControlController {
        // bindings
        onNegotiatorAdded: (obj: { user: Dto.IUser }) => void;
     

        public contactId: string; 
        public leadNegotiator: Business.ContactUser;
        public secondaryNegotiators: Business.ContactUser[] = [];

        public isLeadNegotiatorInEditMode: boolean = false;
        public isSecondaryNegotiatorsInEditMode: boolean = false;

        public negotiatorsSearchOptions: SearchOptions = new SearchOptions();
        public labelTranslationKey: string;
        public nagotiatorCallDateOpened: { [id: string]: boolean; } = {};

        public today: Date = new Date();
        public usersSearchMaxCount: number = 100;

		public negotiatorsForm: IActivityNegotiatorsEditControlForm;

        public inAddMode: boolean = true;

        constructor(
            private $scope: ng.IScope,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService) {

         
        }

      
        public editLeadNegotiator = () => {
            this.isLeadNegotiatorInEditMode = true;
        }

        public changeLeadNegotiator = (user: Dto.IUser) => {
            this.leadNegotiator = this.createContactUser(user);
       
            this.isLeadNegotiatorInEditMode = false;
        }

        public cancelChangeLeadNegotiator = () => {
            this.isLeadNegotiatorInEditMode = false;
        }

        public editSecondaryNegotiators = () => {
            this.isSecondaryNegotiatorsInEditMode = true;
        }

        public addSecondaryNegotiator = (user: Dto.IUser) => {
            this.secondaryNegotiators.push(this.createContactUser(user));
            this.isLeadNegotiatorInEditMode = true;
        }

        public deleteSecondaryNegotiator = (contactUser: Business.ContactUser) => {
            _.remove(this.secondaryNegotiators, (itm) => itm.userId === contactUser.userId);
        }

        public cancelAddSecondaryNegotiator = () => {
            this.isSecondaryNegotiatorsInEditMode = false;
        }

        public switchToLeadNegotiator = (contactUser: Business.ContactUser) => {
            var field = this.negotiatorsForm.callDate;
            if (field.$invalid && field.$dirty) {
                this.leadNegotiator.callDate = null;
            }

            _.remove(this.secondaryNegotiators, (itm) => itm.userId === contactUser.userId);
            this.secondaryNegotiators.push(this.leadNegotiator);

        this.leadNegotiator = contactUser;
          
        }

        public openNegotiatorCallDate = (negotiatorUserId: string) => {
            this.nagotiatorCallDateOpened[negotiatorUserId] = true;
        }

        public getUsersQuery = (searchValue: string): DepartmentUserResourceParameters =>{
            var excludedIds: string[];

            excludedIds = _.map<Business.ContactUser, string>(this.secondaryNegotiators, 'userId');

            if (this.leadNegotiator != null) {
                excludedIds.push(this.leadNegotiator.userId);
            }

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

      
        private createContactUser = (user: Dto.IUser) => {
            var contactUser = new Business.ContactUser();
            contactUser.contactId = this.contactId;
            contactUser.userId = user.id;
            contactUser.user = new Business.User(<Dto.IUser>user);
            return contactUser;
        }
    }

    angular.module('app').controller('ContactNegotiatorsEditControlController', ContactNegotiatorsEditControlController);
}