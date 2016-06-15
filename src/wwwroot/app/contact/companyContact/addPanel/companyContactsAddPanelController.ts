///<reference path="../../../typings/_all.d.ts"/>

module Antares.Ownership {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CompanyContactsAddPanelController extends Common.Component.BaseSidePanelController {
        constructor(private dataAccessService: any) {
            super();
        }

        companyContacts: Business.CompanyContact[] = [];
        initialySelectedCompanyContacts: Business.CompanyContactWithSelection[] = [];
        onSave: (contactCompanies: Business.CompanyContact[]) => void;
        allowMultipleSelect: boolean;
        isOwnershipAddVisible: boolean = false;

        panelShown = () => {
            this.loadCompanyContacts();
            this.isOwnershipAddVisible = false;
        }

        loadCompanyContacts = () => {
            this.isBusy = true;
            this.dataAccessService
                .getCompanyContactResource()
                .query()
                .$promise.then((data: any) => {
                    this.companyContacts = data.map((dataItem: Dto.ICompanyContact) => new Business.CompanyContact(dataItem));

                    this.companyContacts.forEach((current: any) => {
                        var shouldContactBeInitialySelected = _.findIndex(this.initialySelectedCompanyContacts, (initial: Business.CompanyContactWithSelection) => {
                            return current.companyId === initial.companyId && current.contactId === initial.contactId;
                        }) !== -1;
                        current.selected = shouldContactBeInitialySelected;
                    });
                }).finally(() => {
                    this.isBusy = false;
                });
        }
    }

    angular.module('app').controller('CompanyContactsAddPanelController', CompanyContactsAddPanelController);
}