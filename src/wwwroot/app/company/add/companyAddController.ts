/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {

    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class CompanyAddController {

        componentIds: any = {            
            contactSidePanelId: 'addCompany:contactSidePanelComponent',
            contactListId: 'addCompany:contactListComponent'            
        }

        private components: any = {
            sidePanels: {
                contact: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
            },
            contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); }            
        }

        company: Business.Company;
        private companyResource: Antares.Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.ICompanyResource>;

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService) {

            this.company = new Business.Company();
            this.companyResource = dataAccessService.getCompanyResource();

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);                
            });
        }

        hasCompanyContacts = (): boolean => {
            return this.company.contacts != null && this.company.contacts.length > 0;
        }

        showContactList = () => {            
            var selectedContactIds: string[] = this.hasCompanyContacts() ? this.company.contacts.map((contact: Dto.IContact) => { return contact.id }) : [];

            this.components.contactList()
                .loadContacts()
                .then(() => {                    
                    this.components.contactList().setSelected(selectedContactIds);
                    this.components.sidePanels.contact().show();
                })
                .finally(() => {  });
        }

        cancelUpdateContacts = () => {
            this.components.sidePanels.contact().hide();
        }

        updateContacts = () => {
            var selectedContacts = this.components.contactList().getSelected();            
            this.company.contacts = selectedContacts.map((contact: Dto.IContact) => { return new Business.Contact(contact) });
            this.components.sidePanels.contact().hide();
        }

        createCompany = () => {
            this.companyResource
                .save(new Business.CreateCompanyCommand(this.company))
                .$promise
                .then((company: Dto.ICompany) => {                
                    // TODO: replaced with go to view company state
                    alert("Company was saved");
                    this.company = new Business.Company();
                    var form = this.$scope["addCompanyForm"];
                    form.$setPristine()
                    //this.$state.go('app.company-view', company);
                });
        }

    }

    angular.module('app').controller('CompanyAddController', CompanyAddController);
}