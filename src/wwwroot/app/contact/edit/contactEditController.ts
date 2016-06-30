/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {

    import Dto = Common.Models.Dto;
    import IContactTitle = Common.Models.Dto.IContactTitle;
    import Business = Common.Models.Business;

    export class ContactEditController {
  
        // bindings
        public contact: Antares.Common.Models.Dto.IContact;
        userData: Dto.ICurrentUser;


        private contactResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IContactResource>;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService,
            private contactTitleService: Services.ContactTitleService) {

            this.contactResource = dataAccessService.getContactResource();
        }

        $onInit = () => {
            
        }

        setSalutations = () => {

        }

        public cancelEdit(){
            this.$state.go('app.contact-view', this.contact);
        }
     
        public save() {
            
        }
    }

    angular.module('app').controller('ContactEditController', ContactEditController);
}