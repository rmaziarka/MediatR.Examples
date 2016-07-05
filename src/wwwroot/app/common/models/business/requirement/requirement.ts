/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Requirement implements Dto.IRequirement {
        [index: string]: any;

        id: string = '';
        requirementTypeId: string = '';
        createDate: Date = null;
        contacts: Contact[] = [];
        address: Address = new Address();
        description: string;
        rentMin: number = null;
        rentMax: number = null;
        requirementNotes: RequirementNote[] = [];
        viewingsByDay: ViewingGroup[];
        viewings: Viewing[];
        offers: Offer[];
        attachments: Attachment[] = [];
        solicitor: Contact = null;
        solicitorCompany: Company = null;

        constructor(requirement?: Dto.IRequirement) {
            if (requirement) {
                angular.extend(this, requirement);
                this.createDate = Core.DateTimeUtils.convertDateToUtc(requirement.createDate);
                this.contacts = requirement.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
                if (requirement.offers) {
                    this.offers = requirement.offers.map((offer: Dto.IOffer) =>{ return new Offer(offer) });
                }
                this.address = new Address(requirement.address);
                this.requirementNotes = requirement.requirementNotes.map((requirementNote: Dto.IRequirementNote) => { return new RequirementNote(requirementNote) });
                if (requirement.viewings) {
                    this.viewings = requirement.viewings.map((item) => new Viewing(item));
                    this.groupViewings(this.viewings);
                }
                if (requirement.attachments) {
                    this.attachments = requirement.attachments.map((attachment: Dto.IAttachment) => { return new Business.Attachment(attachment) });
                }

                if (requirement.solicitor) {
                    this.solicitor = new Contact(requirement.solicitor);
                }

                if (requirement.solicitorCompany) {
                    this.solicitorCompany = new Company(requirement.solicitorCompany);
                }
            }
        }

        groupViewings(viewings: Viewing[]) {
            this.viewingsByDay = [];
            var groupedViewings: _.Dictionary<Viewing[]> = _.groupBy(viewings, (i: Viewing) => { return i.day; });
            this.viewingsByDay = <ViewingGroup[]>_.map(groupedViewings, (items: Viewing[], key: string) => { return new ViewingGroup(key, items); });
        }

        public getContactNames() {
            return this.contacts.map((c: Contact) => { return c.getName() }).join(", ");
        }
    }
}