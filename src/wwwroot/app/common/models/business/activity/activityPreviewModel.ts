/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business  {
    import Dto = Antares.Common.Models.Dto;

    export class ActivityPreviewModel {
        id: string;
        landlords: Contact[];
        property: PreviewProperty;

        constructor(activity: Dto.IActivity ) {
            this.landlords =  activity.contacts.map((user: Dto.IContact)=> {
                return new Contact(user);
            });
            this.property = new PreviewProperty(activity.property);
            this.id = activity.id;
        }         
    }
}