///<reference path="../../../typings/_all.d.ts"/>

module Antares.Ownership {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CompanyContactsAddPanelController extends Common.Component.BaseSidePanelController {
        constructor(private dataAccessService: any) {
            super();
        }

        contacts: Business.Contact[] = [];
        ownerships: Business.Ownership[];
        onSave: (obj: { ownership: Dto.IOwnership; }) => void;
        allowMultipleSelect: boolean;

        isOwnershipAddVisible: boolean = false;
        propertyId: string;
        selectedContacts: Business.ContactWithSelection[];
        ownershipClear: boolean;

        panelShown = () => {
            this.loadContacts();
            this.isOwnershipAddVisible = false;
            this.ownershipClear = false;
        }

        configure: (contacts: Dto.IContact[]) => void = (contacts: Dto.IContact[]) => {
            this.selectedContacts = contacts.map((c: Dto.IContact) => new Business.ContactWithSelection(c));
            this.ownershipClear = true;
            this.isOwnershipAddVisible = true;
        }

        backOwnership = () => {
            this.isOwnershipAddVisible = false;
        }

        onSavexx = () => {
            this.isOwnershipAddVisible = false;
        }

        saveOwnership = (command: any) => {
            var propertyResource = this.dataAccessService.getPropertyResource();

            return propertyResource
                .createOwnership({ propertyId: this.propertyId }, command)
                .$promise
                .then((ownership: Common.Models.Dto.IOwnership) => {
                    this.ownershipClear = false;
                    this.onSave({ ownership: ownership });
                });
        }

        loadContacts = () => {
            this.isBusy = true;
            this.dataAccessService
                .getContactResource()
                .query()
                .$promise.then((data: any) => {
                    this.contacts = data.map((dataItem: Dto.IContact) => new Business.Contact(dataItem));
                }).finally(() => {
                    this.isBusy = false;
                });
        }
    }

    angular.module('app').controller('CompanyContactsAddPanelController', Antares.Ownership.CompanyContactsAddPanelController);
}