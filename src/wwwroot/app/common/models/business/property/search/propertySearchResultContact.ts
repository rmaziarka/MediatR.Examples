module Antares.Common.Models.Business {
    export class PropertySearchResultContact {
        id: string = null;
        firstName: string = null;
        lastName: string = null;
        title: string = null;

        constructor(propertySearchResultContact?: Dto.IPropertySearchResultContact) {
            if (propertySearchResultContact) {
                this.id = propertySearchResultContact.id;
                this.firstName = propertySearchResultContact.firstName;
                this.lastName = propertySearchResultContact.surname;
                this.title = propertySearchResultContact.title;
            }
        }

        public getName() {
            return this.firstName + ' ' + this.lastName;
        }
    }
}