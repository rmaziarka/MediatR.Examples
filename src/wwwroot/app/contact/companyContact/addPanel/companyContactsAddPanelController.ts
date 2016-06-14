///<reference path="../../../typings/_all.d.ts"/>

module Antares.Ownership {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CompanyContactsAddPanelController extends Common.Component.BaseSidePanelController {
        constructor(private dataAccessService: any) {
            super();
        }

        companyContacts: Business.CompanyContact[] = [];
        onSave: (obj: { ownership: Dto.IOwnership; }) => void;
        allowMultipleSelect: boolean;

        isOwnershipAddVisible: boolean = false;
        selectedContacts: Business.ContactWithSelection[];

        panelShown = () => {
            this.loadCompanyContacts();
            this.isOwnershipAddVisible = false;
        }
        
        backOwnership = () => {
            this.isOwnershipAddVisible = false;
        }

        loadCompanyContacts = () => {
            this.isBusy = true;
            this.dataAccessService
                .getCompanyContactResource()
                .query()
                .$promise.then((data: any) => {
                    this.companyContacts = data.map((dataItem: Dto.ICompanyContact) => new Business.CompanyContact(dataItem));
                }).finally(() => {
                    this.isBusy = false;
                });
        }
    }

    angular.module('app').controller('CompanyContactsAddPanelController', CompanyContactsAddPanelController);
}