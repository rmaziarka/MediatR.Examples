/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    export class ContactAddController {
        public contact: Antares.Common.Models.Dto.IContact;

        private contactResource: Antares.Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IContactResource>;

        constructor(private dataAccessService: Antares.Services.DataAccessService) {
            this.contactResource = dataAccessService.getContactResource();
        }

        public save() {
            this.contactResource.save(this.contact);
        }
    }

    angular.module('app').controller('ContactAddController', ContactAddController);
}