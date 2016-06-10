declare module Antares.Common.Models.Dto {
    interface IOffer {
        id: string;
        activityId: string;
        requirementId: string;
        statusId: string;
        price: number;
        offerDate: Date;
        exchangeDate?: Date;
        completionDate?: Date;
        specialConditions: string;
        negotiatorId: string;
        negotiator: Dto.IUser;
        activity: Dto.IActivity;
        requirement: Dto.IRequirement;
        status: Dto.IEnumTypeItem;
        createdDate?: Date;
        mortgageStatus: Dto.IEnumTypeItem;
        mortgageStatusId: string;
        mortgageSurveyStatus: Dto.IEnumTypeItem;
        mortgageSurveyStatusId: string;
        searchStatus: Dto.IEnumTypeItem; 
        searchStatusId: string;
        enquiries: Dto.IEnumTypeItem; 
        enquiriesId: string;
        contractApproved: boolean;
        mortgageLoanToValue: number;
        broker?: Dto.IContact;
        brokerId: string;
        lender?: Dto.IContact;
        lenderId: string;
        mortgageSurveyDate?: Date;
        surveyor?: Dto.IContact;
        surveyorId: string;
        additionalSurveyor?: Dto.IContact;
        additionalSurveyorId: string;
        additionalSurveyStatus: Dto.IEnumTypeItem;
        additionalSurveyStatusId: string;
        additionalSurveyDate?: Date;
        progressComment: string;
    }
}