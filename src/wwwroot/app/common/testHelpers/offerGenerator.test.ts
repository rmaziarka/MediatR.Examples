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
                pricePerWeek: 100,
                offerDate: new Date(),
                specialConditions: 'special conditions',
                exchangeDate: moment().days(1),
                completionDate: moment().days(3),
                status: <Dto.IEnumTypeItem>{},
                requirement: <Dto.IRequirement>{},
                activity: <Dto.IActivity>{},
                negotiator: <Dto.IUser>{},
                createdDate: new Date(),
                mortgageStatus: <Dto.IEnumTypeItem>{},
                mortgageStatusId: OfferGenerator.makeRandom('mortgageStatusId'),
                mortgageSurveyStatus: <Dto.IEnumTypeItem>{},
                mortgageSurveyStatusId: OfferGenerator.makeRandom('mortgageSurveyStatusId'),
                searchStatus: <Dto.IEnumTypeItem>{},
                searchStatusId: OfferGenerator.makeRandom('searchStatusId'),
                enquiries: <Dto.IEnumTypeItem>{},
                enquiriesId: OfferGenerator.makeRandom('enquiriesId'),
                contractApproved: false,
                mortgageLoanToValue: null,
                broker: <Dto.IContact>{},
                brokerId: OfferGenerator.makeRandom('brokerId'),
                brokerCompany: <Dto.ICompany>{},
                brokerCompanyId: OfferGenerator.makeRandom('brokerCompanyId'),
                lender: <Dto.IContact>{},
                lenderId: OfferGenerator.makeRandom('lenderId'),
                lenderCompany: <Dto.ICompany>{},
                lenderCompanyId: OfferGenerator.makeRandom('lenderCompanyId'),
                mortgageSurveyDate: null,
                surveyor: <Dto.IContact>{},
                surveyorId: OfferGenerator.makeRandom('surveyorId'),
                surveyorCompany: <Dto.ICompany>{},
                surveyorCompanyId: OfferGenerator.makeRandom('surveyorCompanyId'),
                additionalSurveyor: <Dto.IContact> {},
                additionalSurveyorId: OfferGenerator.makeRandom('additionalSurveyorId'),
                additionalSurveyorCompany: <Dto.ICompany>{},
                additionalSurveyorCompanyId: OfferGenerator.makeRandom('additionalSurveyorCompanyId'),
                additionalSurveyStatus: <Dto.IEnumTypeItem>{},
                additionalSurveyStatusId: OfferGenerator.makeRandom('additionalSurveyStatusId'),
                additionalSurveyDate: null,
                progressComment: null
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