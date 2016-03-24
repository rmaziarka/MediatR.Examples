///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        export class ContactListController {
            static $inject = ['componentRegistry', 'dataAccessService'];
            contacts: any = [];
            selected: { [id: string]: any } = {};
            componentId: string;
            isLoading: boolean = false;
            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService) {

                componentRegistry.register(this, this.componentId);
            }

            setSelected = (itemsToSelect: Array<string>) => {
                this.contacts.forEach((c: any) => { c.selected = false; });
                if (itemsToSelect === undefined || itemsToSelect === null || itemsToSelect.length === 0) {
                    return;
                }

                this.contacts.forEach((c: any) => {
                    if (itemsToSelect.indexOf(c.id) > -1) {
                        c.selected = true;
                    }
                });
            }

            getSelected = () => {
                return this.contacts.filter((c: any) => { return c.selected });
            }

            loadContacts = () => {
                this.isLoading = true;
                this.contacts = this.dataAccessService.getContactResource().query();
                return this.contacts.$promise.finally(() => { this.isLoading = false; });
            }
        }

        angular.module('app').controller('contactListController', ContactListController);
    }
}