/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    declare var moment: any;

    export class OfferGenerator {
        public static generateDto(specificData?: any): Dto.IOffer {
            var offer: Dto.IOffer = {
                id: OfferGenerator.makeRandom('id'),
                activityId: OfferGenerator.makeRandom('activityId'),
                requirementId: OfferGenerator.makeRandom('requirementId'),
                statusId: OfferGenerator.makeRandom('statusId'),
                negotiatorId: OfferGenerator.makeRandom('negotiatorId'),
                price: 1000,
                offerDate: new Date(),
                specialConditions: 'special conditions',
                exchangeDate: moment().days(1),
                completionDate: moment().days(3),
                status: <Dto.IEnumTypeItem>{},
                requirement: <Dto.IRequirement>{},
                activity: <Dto.IActivity>{},
                negotiator: <Dto.IUser>{},
                createdDate:new Date()
            };

            offer.requirement.contacts = <Dto.IContact[]>[];
            offer.requirement.requirementNotes = <Dto.IRequirementNote[]>[];
            return angular.extend(offer, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IOffer[] {
            return _.times(n, OfferGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Offer[] {
            return _.map<Dto.IOffer, Business.Offer>(OfferGenerator.generateManyDtos(n), (offer: Dto.IOffer) => { return new Business.Offer(offer); });
        }

        public static generate(specificData?: any): Business.Offer {
            return new Business.Offer(OfferGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}