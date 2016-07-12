/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy  {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    export class ActivityPreviewEditModel {
        id: string;
        landlords: Business.Contact[];
        property: Business.PreviewProperty;

        constructor(activity: Dto.IActivity ) {
            this.landlords =  activity.contacts.map((user: Dto.IContact)=> {
                return new Business.Contact(user);
            });
            this.property = new Business.PreviewProperty(activity.property);
            this.id = activity.id;
        }         
    }
}