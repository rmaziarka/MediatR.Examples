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
                dataAccessService: Antares.Services.DataAccessService){

                componentRegistry.register(this, this.componentId);
                this.contacts = dataAccessService.getContactResource().query();
            }

            getSelected = () => {
                var result = [];
                for (var contactId in this.selected) {
                    if (this.selected.hasOwnProperty(contactId)) {
                        result.push(this.selected[contactId]);
                    }
                }
                return result;
            }

            isSelected(contact){
                return !!this.selected[contact.id];
            }

            toggleSelect(newContact){
                if (this.isSelected(newContact)) {
                    delete this.selected[newContact.id];
                }
                else {
                    this.selected[newContact.id] = newContact;
                }
            }
        }

        angular.module('app').controller('contactListController', ContactListController);

        angular.module('app').component('contactList', {
            controller : 'contactListController',
            controllerAs : 'vm',
            templateUrl : 'app/contact/list/contactList.html',
            transclude: true,
            bindings: {
                componentId: '<'
            }
        });

    }
}