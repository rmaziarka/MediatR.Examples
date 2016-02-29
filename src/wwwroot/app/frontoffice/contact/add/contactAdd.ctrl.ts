/// <reference path="../../../typings/_all.d.ts" />

module Antares.Frontoffice.Contact {
    export class ContactAddController {
        public contact: Common.Models.Dto.IContact;

        private contactResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IContactResource>;

        constructor(private dataAccessService: Services.DataAccessService) {
            this.contactResource = dataAccessService.getContactResource();
        }

        public save() {
            this.contactResource.save(this.contact);
        }
    }

    angular.module('app.frontoffice').controller('ContactAddController', ContactAddController);
}