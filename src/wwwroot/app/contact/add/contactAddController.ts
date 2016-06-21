/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {

    import Dto = Common.Models.Dto;

    export class ContactAddController {
        public contact: Antares.Common.Models.Dto.IContact;
        //mailingSalutations: any;
        //eventSalutations: any;
        //defaultMailingSalutationId: any;
        //defaultEventSalutationId: any;
        //defaultSalutationFormat: string = "";
        userData: Dto.ICurrentUser;
        enumTypeSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.SalutationFormat;
        private salutationFormats: any;
        private currentUserResource: Common.Models.Resources.ICurrentUserResourceClass;

        selectedSalutaionFormatId: string;
        defaultSalutationFormatId: string = "";

        private contactResource: Antares.Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IContactResource>;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService) {

            this.contactResource = dataAccessService.getContactResource();
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        $onInit = () => {
            this.defaultSalutationFormatId = this.userData.salutationFormatId;
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: any) => {
            //find matching code
            var list = result[Dto.EnumTypeCode.SalutationFormat];
            var defaultSalutationFormat: string;

            if (this.defaultSalutationFormatId == undefined || this.defaultSalutationFormatId.length === 0) {
                //set default based on division
                if (this.userData.division.code === "Residential") {
                    defaultSalutationFormat = "JohnSmithEsq";
                }
                else {
                    defaultSalutationFormat = "MrJohnSmith";
                }
            }

            if (defaultSalutationFormat != undefined && defaultSalutationFormat.length > 0) {
                var thisVal = <Dto.IEnumTypeItem>_.find(list, (item: Dto.IEnumTypeItem) => {
                    return item.code === defaultSalutationFormat;
                });

                this.defaultSalutationFormatId = thisVal.id;
            }
        }
        setSalutations = () => {
            this.contact.mailingSemiformalSalutation = ((this.contact.title||"") + " " + (this.contact.lastName||"")).trim();

            this.contact.mailingInformalSalutation = (this.contact.firstName && this.contact.firstName.length > 1) ?
                this.contact.firstName : ((this.contact.title || "") + " " + (this.contact.lastName || "")).trim();

            this.contact.mailingFormalSalutation = (this.contact.title && this.contact.title.toLowerCase() == "mr") ? "Sir" :
                ((this.contact.title && (this.contact.title.toLowerCase() == "mrs" || this.contact.title.toLowerCase() == "ms" || this.contact.title.toLowerCase() == "miss")) ? "Madam" :
                        ((this.contact.title || "") + " " + (this.contact.lastName || "")).trim());

            this.contact.mailingEnvelopeSalutation = ((this.contact.title && this.contact.title.toLowerCase() == "mr" && this.defaultSalutationFormat == "") ? 
                ((this.contact.firstName || "") + " " + (this.contact.lastName || "") + ", Esq").trim() :
                (((this.contact.title || "") + " " + (this.contact.firstName || "")).trim() + " " + (this.contact.lastName || "")).trim());
        }

        public save() {
            this.contact.defaultMailingSalutationId = this.defaultMailingSalutationId != null ? this.defaultMailingSalutationId.id : "";
            this.contact.defaultEventSalutationId = this.defaultEventSalutationId != null ? this.defaultEventSalutationId.id : "";
            this.contactResource.save(this.contact);
            var form = this.$scope["addContactForm"];
            form.$setPristine();
        }
    }

    angular.module('app').controller('ContactAddController', ContactAddController);
}