///<reference path="../../../typings/main.d.ts"/>
/// <reference path="../../common/core/services/registry/componentRegistryService.ts" />

module Antares {
    export module Component {
        export class ContactListController {
            static $inject = ['componentRegistry', 'dataAccessService'];
            contacts: any;
            selected: { [id: string]: any } = {};
            componentId: string;
            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                dataAccessService: Antares.Services.DataAccessService) {

                componentRegistry.register(this, this.componentId);
                this.contacts = dataAccessService.getContactResource().query();
            }

            setSelected = (itemsToSelect: Array<string>) => {
                this.contacts.forEach(c => { c.selected = false; });
                if (itemsToSelect === undefined || itemsToSelect === null || itemsToSelect.length === 0) {
                    return;
                }

                this.contacts.forEach(c => {
                    if (itemsToSelect.indexOf(c.id) > -1) {
                        c.selected = true;
                    }
                })
            }

            getSelected = () => {
                var result = [];
                return this.contacts.filter(c => { return c.selected });
            }
        }

        angular.module('app').controller('contactListController', ContactListController);

        angular.module('app').component('contactList', {
            controller: 'contactListController',
            controllerAs: 'vm',
            templateUrl: 'app/contact/list/contactList.html',
            transclude: true,
            bindings: {
                componentId: '<'
            }
        });

    }
}