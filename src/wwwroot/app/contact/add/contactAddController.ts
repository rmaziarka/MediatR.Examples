/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {

    import Dto = Common.Models.Dto;
    import IContactTitle = Common.Models.Dto.IContactTitle;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ContactAddController {
        public config: Attributes.IContactNegotiatorsControlConfig;
        public contact: Antares.Common.Models.Dto.IContact = new Business.Contact();
         
        public searchOptions: Common.Component.SearchOptions = new Common.Component.SearchOptions({ minLength: 0, isEditable: true, nullOnSelect: false, showCancelButton: false, isRequired: true, maxLength: 128  });

        userData: Dto.ICurrentUser;
        mailingSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.MailingSalutation;
        eventSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.EventSalutation;
      
        private currentUserResource: Common.Models.Resources.ICurrentUserResourceClass;

        defaultSalutationFormat: string = "";
        defaultSalutationFormatId: string = "";
        defaultMailingSalutationId: string = "";
        defaultEventSalutationId: string = "";

        private contactTitles: IContactTitle[];

        public selectedTitle: string;

        private contactResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IContactResource>;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService,
            private contactTitleService: Services.ContactTitleService) {

            this.contactResource = dataAccessService.getContactResource();

        }

        $onInit = () => {
            this.defaultSalutationFormatId = this.userData.salutationFormatId;
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
            this.contactTitleService.get().then((contactTitles) => {
                this.contactTitles = contactTitles.data;
            });

            this.setDefaultLeadNegotiator(this.userData,this.contact);
           
        }
     
        setDefaultLeadNegotiator = (currentUser:Dto.ICurrentUser, contact:Dto.IContact) =>{ 
           var  defaultUser: Dto.IUser = {
               id : currentUser.id,
               firstName : currentUser.firstName,
               lastName : currentUser.lastName,
               departmentId : "",
               department : null
           }

        

            var defaultLeadNegotiator = new Business.ContactUser();
            defaultLeadNegotiator.user = new Business.User(<Dto.IUser>defaultUser);
            defaultLeadNegotiator.contactId = this.contact.id;
            defaultLeadNegotiator.userId = this.userData.id;
            contact.leadNegotiator = defaultLeadNegotiator;
        }
        public getContactTitles = (typedTitle: string) => {

            var locale = this.userData.locale.isoCode;
            var items: IContactTitle[];

            if (typedTitle) {
                items = this.contactTitles.filter((item) => { return item.title.toLowerCase().indexOf(typedTitle.toLowerCase()) > -1; }); // Filter by entered text (regardless of locale)
                items = _.sortBy(items, item => [item.title]); // Order alpabetically
            }
            else {
                items = this.contactTitles.filter((item) => { return item.locale.isoCode.toLowerCase().indexOf(locale.toLowerCase()) > -1; }); // Filter by current locale
                items = _.sortBy(items, item => [item.priority || Number.POSITIVE_INFINITY, item.title]); // Order by priority (if null, set to infinity to place last), then alpabetically
            }

            return items.map((item) => item.title);
        }

        public contactTitleSelect = (contactTitle: string) => {
            this.selectedTitle = contactTitle;
            this.setSalutations();
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

            if (!this.contact) return; // Note, due to us not being able to bind contact.title at present, this is necessary

            var title = this.selectedTitle;

            this.contact.mailingSemiformalSalutation = ((title || "") + " " + (this.contact.lastName || "")).trim();

            this.contact.mailingInformalSalutation = (this.contact.firstName && this.contact.firstName.length > 1) ?
                this.contact.firstName : ((title || "") + " " + (this.contact.lastName || "")).trim();

            this.contact.mailingFormalSalutation = (title && title.toLowerCase() == "mr") ? "Sir" :
                ((title && (title.toLowerCase() == "mrs" || title.toLowerCase() == "ms" || title.toLowerCase() == "miss")) ? "Madam" :
                    ((title || "") + " " + (this.contact.lastName || "")).trim());

            this.contact.mailingEnvelopeSalutation = ((title && title.toLowerCase() == "mr" && this.defaultSalutationFormat == "JohnSmithEsq") ?
                ((this.contact.firstName || "") + " " + (this.contact.lastName || "") + ", Esq").trim() :
                (((title || "") + " " + (this.contact.firstName || "")).trim() + " " + (this.contact.lastName || "")).trim());
        }

        public save(){
            this.contact.defaultMailingSalutationId = this.defaultMailingSalutationId != null ? this.defaultMailingSalutationId : "";
            this.contact.defaultEventSalutationId = this.defaultEventSalutationId != null ? this.defaultEventSalutationId : "";

            if (this.selectedTitle) {
                this.contact.title = this.selectedTitle;
            }

            this.contactResource.save(this.contact)
                .$promise
                .then((contact: Dto.IContact) =>{
                    this.contact = new Business.Contact();
                    var form = this.$scope["addContactForm"];
                    form.$setPristine();
                    this.$state.go('app.contact-view', contact);
                });
        }
    }

    angular.module('app').controller('ContactAddController', ContactAddController);
}