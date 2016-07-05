/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {

    import Dto = Common.Models.Dto;

    export class EditSalutationsController {

        // bindings
        public contact: Dto.IContact;
        public firstName: string;
        public lastName: string;
        public title: string;
        public defaultSalutationFormatId: string;

        // form
        public salutationsForm: ng.IFormController | any; // injected from the view

        // fields
        mailingSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.MailingSalutation;
        eventSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.EventSalutation;
        defaultSalutationFormat: string = "";

        constructor(private enumProvider: Providers.EnumProvider) {
        }

        public $onInit = () => {
            let salutationFormats: Dto.IEnumItem[] = this.enumProvider.enums[Dto.EnumTypeCode.SalutationFormat];

            let defaultSalutationFormat = salutationFormats.filter((format: Dto.IEnumItem) => {
                return format.id === this.defaultSalutationFormatId;
            })[0];

            if (defaultSalutationFormat) {
                this.defaultSalutationFormat = defaultSalutationFormat.code || "";
            }
        }
        
        public $onChanges = (changesObj: IEditSalutationsChange) => {
            if (this.hasBindingChanged(changesObj.firstName)
                || this.hasBindingChanged(changesObj.lastName)
                || this.hasBindingChanged(changesObj.title)) {
                this.setSalutations();
            }
        }
        
        private hasBindingChanged = (binding: IBindingChange<any>): boolean => {
            if (!binding) {
                return false;
            }
            return binding.isFirstChange() === false && (binding.currentValue !== binding.previousValue); 
        }
        
        private setSalutations = () => {
            // TODO: copied from "ContactAddController.setSalutations()" - make common
            if (!this.contact) return; // Note, due to us not being able to bind contact.title at present, this is necessary

            let title = this.title;

            this.contact.mailingSemiformalSalutation = ((title || "") + " " + (this.contact.lastName || "")).trim();

            this.contact.mailingInformalSalutation = (this.contact.firstName && this.contact.firstName.length > 1) ?
                this.contact.firstName : ((title || "") + " " + (this.contact.lastName || "")).trim();

            this.contact.mailingFormalSalutation = (title && title.toLowerCase() === "mr") ? "Sir" :
                ((title && (title.toLowerCase() === "mrs" || title.toLowerCase() === "ms" || title.toLowerCase() === "miss")) ? "Madam" :
                    ((title || "") + " " + (this.contact.lastName || "")).trim());

            this.contact.mailingEnvelopeSalutation = ((title && title.toLowerCase() === "mr" && this.defaultSalutationFormat === "JohnSmithEsq") ?
                ((this.contact.firstName || "") + " " + (this.contact.lastName || "") + ", Esq").trim() :
                (((title || "") + " " + (this.contact.firstName || "")).trim() + " " + (this.contact.lastName || "")).trim());
        }
    }

    interface IBindingChange<T> {
        currentValue: T;
        previousValue: T;
        isFirstChange: () => boolean;
    }

    interface IEditSalutationsChange {
        firstName: IBindingChange<string>;
        lastName: IBindingChange<string>;
        title: IBindingChange<string>;
    }

    angular.module('app').controller('EditSalutationsController', EditSalutationsController);
}