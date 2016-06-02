/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {

    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class CompanyAddController extends Core.WithPanelsBaseController  {
        company: Business.Company;
        private companyResource: Antares.Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.ICompanyResource>;
        clientCareStatuses: any;
        selectedStatus: any;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService) {

            super(componentRegistry, $scope);

            this.company = new Business.Company();
            this.companyResource = dataAccessService.getCompanyResource();
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: any) =>{
            this.clientCareStatuses = result[Dto.EnumTypeCode.ClientCareStatus];
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
                });
        }

        cancelUpdateContacts = () => {
            this.components.sidePanels.contact().hide();
        }

        updateContacts = () => {
            var selectedContacts = this.components.contactList().getSelected();            
            this.company.contacts = selectedContacts.map((contact: Dto.IContact) => { return new Business.Contact(contact) });
            this.components.sidePanels.contact().hide();
        }
     
        formatUrlWithProtocol = (url:string):string=> {
            //regular expression for url with a protocol (case insensitive)
            var r = new RegExp('^(?:[a-z]+:)?//', 'i');
            if (r.test(url)) { return url; }
            else { return 'http://' + url; }
        }
   
        createCompany = () => {
        
            this.company.clientCareStatusId = this.selectedStatus.id;
            this.company.websiteUrl = this.formatUrlWithProtocol(this.company.websiteUrl);
            this.company.clientCarePageUrl = this.formatUrlWithProtocol(this.company.clientCarePageUrl);
            this.companyResource
                .save(new Business.CreateCompanyResource(this.company))
                .$promise
                .then((company: Dto.ICompany) => {                
                   this.company = new Business.Company();
                    var form = this.$scope["addCompanyForm"];
                    form.$setPristine();
                    this.$state.go('app.company-view', company);
                });
        }

        defineComponentIds() {
            this.componentIds = {
                contactSidePanelId: 'addCompany:contactSidePanelComponent',
                contactListId: 'addCompany:contactListComponent'
            }
        }

        defineComponents() {
            this.components = {
                sidePanels: {
                    contact: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                },
                contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); }            
            };
        }

    }

    angular.module('app').controller('CompanyAddController', CompanyAddController);
}