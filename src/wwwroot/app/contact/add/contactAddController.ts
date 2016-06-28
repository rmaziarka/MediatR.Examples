/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {

    import Dto = Common.Models.Dto;
    import IContactTitle = Common.Models.Dto.IContactTitle;

    export class ContactAddController {
        public contact: Antares.Common.Models.Dto.IContact;
        public searchOptions: Common.Component.SearchOptions = new Common.Component.SearchOptions({ minLength: 0, isEditable : true });

        userData: Dto.ICurrentUser;
        mailingSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.MailingSalutation;
        eventSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.EventSalutation;
        private currentUserResource: Common.Models.Resources.ICurrentUserResourceClass;

        defaultSalutationFormat: string = "";
        defaultSalutationFormatId: string = "";
        defaultMailingSalutationId: string = "";
        defaultEventSalutationId: string = "";

        private contactTitles: IContactTitle[];

        private selectedTitle: string;

        private contactResource: Antares.Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IContactResource>;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService,
            private contactTitleService: Antares.Services.ContactTitleService) {

            this.contactResource = dataAccessService.getContactResource();
        }

        $onInit = () => {
            this.defaultSalutationFormatId = this.userData.salutationFormatId;
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
            this.contactTitleService.get().then((contactTitles) =>{
                this.contactTitles = contactTitles.data;
            });
        }

        public getContactTitles = (typedTitle: string) =>{

            var locale = this.userData.locale.isoCode;
            var items : IContactTitle[];

            if (typedTitle) {
                items = this.contactTitles.filter((item) => { return item.title.toLowerCase().indexOf(typedTitle.toLowerCase()) > -1; }); // Filter by entered text (regardless of locale)
                items = _.sortBy(items, function (item) { return [item.title] }); // Order alpabetically
            }
            else {
                items = this.contactTitles.filter((item) => { return item.locale.isoCode.toLowerCase().indexOf(locale.toLowerCase()) > -1; }); // Filter by current locale
                items = _.sortBy(items, function (item) { return [item.priority || Number.POSITIVE_INFINITY, item.title] }); // Order by priority (if null, set to infinity to place last), then alpabetically
            }

            return items.map((item) => item.title);
        }

        public contactTitleSelect = (contactTitle: string) => {

            // TODO - Fire on non-select (i.e. free text entry)
            this.selectedTitle = contactTitle;
        }

        onEnumLoaded = (result: any) => {
            var mailingList = result[Dto.EnumTypeCode.MailingSalutation];
            var eventList = result[Dto.EnumTypeCode.EventSalutation];
            var defaultFormatList = result[Dto.EnumTypeCode.SalutationFormat];

            var defaultMailingSalutation: string = "MailingSemiformal";
            var defaultEventSalutation: string = "EventSemiformal";

            var mailingVal = <Dto.IEnumTypeItem>_.find(mailingList, (item: Dto.IEnumTypeItem) => {
                return item.code === defaultMailingSalutation;
            });

            var eventVal = <Dto.IEnumTypeItem>_.find(eventList, (item: Dto.IEnumTypeItem) => {
                return item.code === defaultEventSalutation;
            });

            var defaultFormat = <Dto.IEnumTypeItem>_.find(defaultFormatList, (item: Dto.IEnumTypeItem) => {
                return item.id === this.defaultSalutationFormatId;
            });

            this.defaultMailingSalutationId = mailingVal.id;
            this.defaultEventSalutationId = eventVal.id;

            if (defaultFormat != null)
                this.defaultSalutationFormat = (defaultFormat.code || "");
        }

        setSalutations = () => {
            this.contact.mailingSemiformalSalutation = ((this.contact.title || "") + " " + (this.contact.lastName || "")).trim();

            this.contact.mailingInformalSalutation = (this.contact.firstName && this.contact.firstName.length > 1) ?
                this.contact.firstName : ((this.contact.title || "") + " " + (this.contact.lastName || "")).trim();

            this.contact.mailingFormalSalutation = (this.contact.title && this.contact.title.toLowerCase() == "mr") ? "Sir" :
                ((this.contact.title && (this.contact.title.toLowerCase() == "mrs" || this.contact.title.toLowerCase() == "ms" || this.contact.title.toLowerCase() == "miss")) ? "Madam" :
                    ((this.contact.title || "") + " " + (this.contact.lastName || "")).trim());

            this.contact.mailingEnvelopeSalutation = ((this.contact.title && this.contact.title.toLowerCase() == "mr" && this.defaultSalutationFormat == "JohnSmithEsq") ?
                ((this.contact.firstName || "") + " " + (this.contact.lastName || "") + ", Esq").trim() :
                (((this.contact.title || "") + " " + (this.contact.firstName || "")).trim() + " " + (this.contact.lastName || "")).trim());
        }

        public save() {
            this.contact.defaultMailingSalutationId = this.defaultMailingSalutationId != null ? this.defaultMailingSalutationId : "";
            this.contact.defaultEventSalutationId = this.defaultEventSalutationId != null ? this.defaultEventSalutationId : "";

            if (this.selectedTitle) {
                this.contact.title = this.selectedTitle;
            }

            this.contactResource.save(this.contact);
            var form = this.$scope["addContactForm"];
            form.$setPristine();
        }
    }

    angular.module('app').controller('ContactAddController', ContactAddController);
}