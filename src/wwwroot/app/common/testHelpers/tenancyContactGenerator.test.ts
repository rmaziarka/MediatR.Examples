/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class TenancyContactGenerator {
        public static generateDto(tenancyContactType: Enums.TenancyContactType): Dto.ITenancyContact {
            var contact = ContactGenerator.generateDto();

            var contactType: Dto.IEnumTypeItem = EnumTypeItemGenerator.generateDto(Enums.TenancyContactType[tenancyContactType]);

            var tenancyContact: Dto.ITenancyContact = {
                id: StringGenerator.generate(),
                tenancyId: StringGenerator.generate(),
                contact: contact,
                contactId: contact.id,
                contactType: contactType
            }

            return tenancyContact;
        }

        public static generateManyDtos(n: number, tenancyContactType: Enums.TenancyContactType): Dto.ITenancyContact[] {
            return _.times(n, TenancyContactGenerator.generateDto, tenancyContactType);
        }
    }
}