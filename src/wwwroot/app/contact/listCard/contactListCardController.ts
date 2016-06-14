///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Business = Common.Models.Business;
        import IContact = Antares.Common.Models.Dto.IContact;
        import CloseSidePanelMessage = Antares.Common.Component.CloseSidePanelMessage;

        export class ContactListCardController {
            constructor(private pubSub: Core.PubSub) { }
            allowMultipleSelect: boolean;
            contacts: Business.ContactWithSelection[];
            onSave: (contacts: IContact[]) => void;
            onConfigure: (contacts: { contacts: IContact[]; }) => void;

            close = () => {
                this.pubSub.publish(new CloseSidePanelMessage());
            }

            cardSelected = (contact: any, selected: boolean) => {
                var selectedContact = this.contacts.filter((c: any) => {
                    return c.id === contact.id;
                })[0];
                selectedContact.selected = selected;

                if (!this.allowMultipleSelect && selected) {
                    this.contacts.forEach((c: any) => {
                        if (c.selected && c.id !== contact.id) {
                            c.selected = false;
                        }
                    });
                }
            }

            save = () =>{
                var selectedContacts = this.contacts
                    .filter((c: any) => c.selected);

                this.onSave(selectedContacts);
            }
        }

        angular.module('app').controller('contactListCardController', ContactListCardController);

    }
}