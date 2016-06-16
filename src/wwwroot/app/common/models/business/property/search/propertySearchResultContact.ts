module Antares.Common.Models.Business {
    export class PropertySearchResultContact {
        id: string = null;
        firstName: string = null;
        surname: string = null;
        title: string = null;

        constructor(propertySearchResultContact?: Dto.IPropertySearchResultContact) {
            if (propertySearchResultContact) {
                this.id = propertySearchResultContact.id;
                this.firstName = propertySearchResultContact.firstName;
                this.surname = propertySearchResultContact.surname;
                this.title = propertySearchResultContact.title;
            }
        }
    }
}