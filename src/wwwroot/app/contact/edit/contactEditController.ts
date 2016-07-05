/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {

    import Dto = Common.Models.Dto;
    import IContactTitle = Common.Models.Dto.IContactTitle;
    import Business = Common.Models.Business;

    export class ContactEditController {
  
        // bindings
        public contact: Dto.IContact;
        userData: Dto.ICurrentUser;

        // fields
        public editContactForm: ng.IFormController | any; // injected from the view
        public defaultSalutationFormatId: string = "";

        public searchOptions: Common.Component.SearchOptions = new Common.Component.SearchOptions({ minLength: 0, isEditable: true, nullOnSelect: false, showCancelButton: false, isRequired: true, maxLength: 128 });

        private contactTitles: IContactTitle[];

        private selectedTitle: string;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService,
            private kfMessageService: Services.KfMessageService,
            private contactTitleService: Services.ContactTitleService) {
        }

        $onInit() {
            this.defaultSalutationFormatId = this.userData.salutationFormatId;
            this.contactTitleService.get().then((contactTitles) => {
                this.contactTitles = contactTitles.data;
            });

            this.selectedTitle = this.contact.title;
        }

        public getContactTitles = (typedTitle: string) => {
            if (typedTitle) {
                return this.handleTypedTitle(typedTitle);
            } else {
                return this.handleEmptyTitle();
            }
        }

        private handleTypedTitle = (typedTitle: string) => {
            var localeContactTitles = this.filterByTypedTitle(this.contactTitles, typedTitle);
            var sortedLocaleContactTitles = this.sortContactTitlesAlpabetically(localeContactTitles);

            return sortedLocaleContactTitles.map((item) => item.title);
        }

        private filterByTypedTitle = (contactTitles: IContactTitle[], typedTitle: string) => {
            return contactTitles.filter((item) => {
                return item.title.toLowerCase().indexOf(typedTitle.toLowerCase()) > -1; //Regardless of locale
            });
        }

        private sortContactTitlesAlpabetically = (contactTitles: IContactTitle[]) => {
            return _.sortBy(contactTitles, item => item.title);
        }

        private handleEmptyTitle = () => {
            var localeContactTitles = this.filterByCurrentLocale(this.contactTitles);
            var sortedLocaleContactTitles = this.sortContactTitleByPriorityThenAlphabetically(localeContactTitles);

            return sortedLocaleContactTitles.map((item) => item.title);
        }

        private filterByCurrentLocale = (contactTitles: IContactTitle[]) => {
            var locale = this.userData.locale.isoCode;

            return contactTitles.filter((item) => {
                return item.locale.isoCode.toLowerCase().indexOf(locale.toLowerCase()) > -1;
            });
        }

        private sortContactTitleByPriorityThenAlphabetically = (contactTitles: IContactTitle[]) => {
            return _.sortBy(contactTitles, item => [item.priority || Number.POSITIVE_INFINITY, item.title]); // if null, set to infinity to place last
        }

        public contactTitleSelect = (contactTitle: string) => {
            this.selectedTitle = contactTitle;
        }
        
        public cancelEdit(){
            this.redirectToView();
        }
     
        public save(){
            const editedContact: Dto.IContact = angular.copy(this.contact);
            editedContact.title = this.selectedTitle;

            const contactResource = this.dataAccessService.getContactResource();

            return contactResource
                .update(editedContact)
                .$promise
                .then((contact: Dto.IContact) => {
                    this.editContactForm.$setPristine();
                    this.redirectToView()
                        .then(() => this.kfMessageService.showSuccessByCode('CONTACT.EDIT.CONTACT_EDIT_SUCCESS'));;
                }, (response: any) => {
                    this.kfMessageService.showErrors(response);
                });
        }

        private redirectToView() : ng.IPromise<any> {
            return this.$state.go('app.contact-view', this.contact);
        }
    }

    angular.module('app').controller('ContactEditController', ContactEditController);
}