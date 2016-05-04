/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Requirement implements Dto.IRequirement {
        [index: string]: any;

        id: string = '';
        createDate: Date = null;
        contacts: Contact[] = [];
        address: Address = new Address();
        description: string;
        requirementNotes: RequirementNote[] = [];
        viewingsByDay: ViewingGroup[];
        viewings: Viewing[];

        constructor(requirement?: Dto.IRequirement) {
            if (requirement) {
                angular.extend(this, requirement);
                this.viewings = requirement.viewings.map((item) => new Viewing(item));
                this.createDate = Core.DateTimeUtils.convertDateToUtc(requirement.createDate);
                this.contacts = requirement.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
                this.address = new Address(requirement.address);
                this.requirementNotes = requirement.requirementNotes.map((requirementNote: Dto.IRequirementNote) => { return new RequirementNote(requirementNote) });
                this.groupViewings(this.viewings);                
            }
        }
                
        groupViewings(viewings: Viewing[]) {
            this.viewingsByDay = [];
            var groupedViewings: _.Dictionary<Viewing[]> = _.groupBy(viewings, (i: Viewing) => { return i.day; });
            this.viewingsByDay = _.map(groupedViewings, (items: Viewing[], key: string) => { return new ViewingGroup(key, items); });            
        }
    }
}