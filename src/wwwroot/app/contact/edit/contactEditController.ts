/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {

    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ContactEditController {
  
        // bindings
        public contact: Dto.IContact;
        userData: Dto.ICurrentUser; //todo is required?

        // fields
        public editContactForm: ng.IFormController | any; // injected from the view

        constructor(
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService,
            private contactTitleService: Services.ContactTitleService) {//todo!remove unused params

            //this.contactResource = dataAccessService.getContactResource();

            //todo - move to computed field: (editContactForm.$submitted || editContactForm.title.$dirty) && editContactForm.title.$invalid
        }

        $onInit = () => {
        }

        setSalutations = () => {

        }

        public cancelEdit(){
            this.redirectToView();
        }
     
        public save(){
            const editedContact: Dto.IContact = angular.copy(this.contact);

            const contactResource = this.dataAccessService.getContactResource();

            return contactResource
                .update(editedContact)
                .$promise
                .then((contact: Dto.IContact) => {
                    this.editContactForm.$setPristine();
                    this.redirectToView();
                }, (response: any) => {
                    // todo show errors
                });
        }

        private redirectToView() : ng.IPromise<any> {
            return this.$state.go('app.contact-view', this.contact);
        }
    }

    angular.module('app').controller('ContactEditController', ContactEditController);
}