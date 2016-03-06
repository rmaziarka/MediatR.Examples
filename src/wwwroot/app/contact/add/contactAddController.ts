/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    export class ContactAddController {
        public contact: Common.Models.Dto.IContact;

        private contactResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IContactResource>;

        constructor(private dataAccessService: Services.DataAccessService) {
            this.contactResource = dataAccessService.getContactResource();
            //console.log(kfconfig);
        }

        public save() {
            this.contactResource.save(this.contact);
        }
    }

    angular.module('app').controller('ContactAddController', ContactAddController);
}