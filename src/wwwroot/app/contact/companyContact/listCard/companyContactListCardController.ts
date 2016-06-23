///<reference path="../../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Business = Common.Models.Business;
        import IContact = Antares.Common.Models.Dto.IContact;
        import ICompanyContact = Antares.Common.Models.Dto.ICompanyContact;

        export class CompanyContactListCardController {
            constructor(private eventAggregator: Core.EventAggregator) { }
            allowMultipleSelect: boolean;
            contacts: Business.CompanyContactWithSelection[];
            onSave: (contacts: ICompanyContact[]) => void;
            onConfigure: (contacts: { contacts: IContact[]; }) => void;

            close = () => {
                this.eventAggregator.publish(new Common.Component.CloseSidePanelEvent());
            }

            cardSelected = (companyContact: any, selected: boolean) => {
                if (!this.allowMultipleSelect && selected) {
                    this.contacts.forEach((c: any) => {
                        c.selected = false;
                    });
                }

                var selectedContact = this.contacts.filter((c: any) => {
                    return c.contact.id === companyContact.contact.id && c.company.id === companyContact.company.id;
                })[0];
                selectedContact.selected = selected;
            }

            save = () => {
                var selectedContacts = this.contacts
                    .filter((c: any) => c.selected);

                this.onSave(selectedContacts);
            }
        }

        angular.module('app').controller('companyContactListCardController', CompanyContactListCardController);

    }
}