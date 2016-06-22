module Antares.Common.Models.Business {

    export class PropertyView {
        id: string = null;
        propertyTypeId: string = null;
        address: Business.Address = new Business.Address();
        ownerships: Business.Ownership[] = [];
        activities: Business.Activity[] = [];
        attachments: Attachment[] = [];
        division: Business.EnumTypeItem = <Business.EnumTypeItem>{};
        attributeValues: any = {};
        // dynamic object created basing on list of characteristic (with characteristicId as key)
        propertyCharacteristicsMap: any = {};
        propertyAreaBreakdowns: Business.PropertyAreaBreakdown[] = [];

        constructor(property?: Dto.IProperty) {
            if (property) {
                this.id = property.id;
                this.address = new Business.Address();
                angular.extend(this.address, property.address);

                this.ownerships = property.ownerships.map((ownership: Dto.IOwnership) => { return new Business.Ownership(ownership) });
                this.activities = property.activities.map((activity: Dto.IActivity) => { return new Business.Activity(activity) });
                this.propertyAreaBreakdowns = property.propertyAreaBreakdowns.map((area: Dto.IPropertyAreaBreakdown) => { return new Business.PropertyAreaBreakdown(area) });

                angular.extend(this.division, property.division);

                this.propertyTypeId = property.propertyTypeId;
                this.attributeValues = property.attributeValues;

                if (property.attachments) {
                    this.attachments = property.attachments.map((attachment: Dto.IAttachment) => { return new Business.Attachment(attachment) });
                }

                _.reduce(property.propertyCharacteristics, (propertyCharacteristicObject, characteristicItem) => {
                    propertyCharacteristicObject[characteristicItem.characteristicId] = new CharacteristicSelect(characteristicItem);
                    return propertyCharacteristicObject;
                }, this.propertyCharacteristicsMap);
            }
        }

        get totalAreaBreakdown(): number {
            return _.sum(this.propertyAreaBreakdowns, (area: Business.PropertyAreaBreakdown) => area.size);
        }

        isCommercial(): boolean {
            return this.division.code === Models.Dto.DivisionEnumTypeCode[Models.Dto.DivisionEnumTypeCode.Commercial];
        }
    }
}