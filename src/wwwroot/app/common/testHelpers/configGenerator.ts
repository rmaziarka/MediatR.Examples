/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    export class ConfigGenerator {
        public static generateOfferOfferDateConfig(): Attributes.IOfferOfferDateControlConfig {
            var config: Attributes.IOfferOfferDateControlConfig = {
                offerDate: null
            }

            return config;
        }

        public static generateOfferExchangeDateConfig(): Attributes.IOfferExchangeDateControlConfig {
            var config: Attributes.IOfferExchangeDateControlConfig = {
                exchangeDate: null
            }

            return config;
        }

        public static generateOfferCompletionDateConfig(): Attributes.IOfferCompletionDateControlConfig {
            var config: Attributes.IOfferCompletionDateControlConfig = {
                completionDate: null
            }

            return config;
        }

        public static generateOfferAdditionalSurveyDateConfig(): Attributes.IOfferAdditionalSurveyDateControlConfig {
            var config: Attributes.IOfferAdditionalSurveyDateControlConfig = {
                additionalSurveyDate: null
            }

            return config;
        }

        public static generateOfferMortgageSurveyDateConfig(): Attributes.IOfferMortgageSurveyDateControlConfig {
            var config: Attributes.IOfferMortgageSurveyDateControlConfig = {
                mortgageSurveyDate: null
            }

            return config;
        }

        public static generateOfferStatusIdConfig(): Attributes.IOfferStatusControlConfig {
            var config: Attributes.IOfferStatusControlConfig = {
                statusId: null
            }

            return config;
        }

        public static generateOfferPriceConfig(): Attributes.IOfferPriceControlConfig {
            var config: Attributes.IOfferPriceControlConfig = {
                price: null
            }

            return config;
        }

        public static generateOfferPricePerWeekConfig(): Attributes.IOfferPricePerWeekControlConfig {
            var config: Attributes.IOfferPricePerWeekControlConfig = {
                pricePerWeek: null
            }

            return config;
        }

        public static generateOfferSpecialConditionsConfig(): Attributes.IOfferSpecialConditionsControlConfig {
            var config: Attributes.IOfferSpecialConditionsControlConfig = {
                specialConditions: null
            }

            return config;
        }

        public static generateOfferCommentConfig(): Attributes.IOfferProgressCommentControlConfig {
            var config: Attributes.IOfferProgressCommentControlConfig = {
                progressComment: null
            }

            return config;
        }

        public static generateOfferBrokerConfig(): Attributes.IOfferBrokerControlConfig {
            var config: Attributes.IOfferBrokerControlConfig = {
                brokerId: null
            }

            return config;
        }
        public static generateOfferAdditionalSurveyorConfig(): Attributes.IOfferAdditionalSurveyorControlConfig {
            var config: Attributes.IOfferAdditionalSurveyorControlConfig = {
                additionalSurveyorId: null
            }

            return config;
        }

        public static generateOfferLenderConfig(): Attributes.IOfferLenderControlConfig {
            var config: Attributes.IOfferLenderControlConfig = {
                lenderId: null
            }

            return config;
        }

        public static generateOfferSurveyorConfig(): Attributes.IOfferSurveyorControlConfig {
            var config: Attributes.IOfferSurveyorControlConfig = {
                surveyorId: null
            }

            return config;
        }

        public static generateOfferContractApprovedConfig(): Attributes.IOfferContractApprovedControlConfig {
            var config: Attributes.IOfferContractApprovedControlConfig = {
                contractApproved: null
            }

            return config;
        }

        public static generateOfferEnquiriesConfig(): Attributes.IOfferEnquiriesControlConfig {
            var config: Attributes.IOfferEnquiriesControlConfig = {
                enquiriesId: null
            }

            return config;
        }

        public static generateOfferMortgageStatusConfig(): Attributes.IOfferMortgageStatusControlConfig {
            var config: Attributes.IOfferMortgageStatusControlConfig = {
                mortgageStatusId: null
            }

            return config;
        }

        public static generateOfferMortgageSurveyStatusConfig(): Attributes.IOfferMortgageSurveyStatusControlConfig {
            var config: Attributes.IOfferMortgageSurveyStatusControlConfig = {
                mortgageSurveyStatusId: null
            }

            return config;
        }

        public static generateOfferSearchStatusConfig(): Attributes.IOfferSearchStatusControlConfig {
            var config: Attributes.IOfferSearchStatusControlConfig = {
                searchStatusId: null
            }

            return config;
        }
        public static generateOfferAddPanelConfig(): Offer.IOfferAddPanelConfig{
            var config: Offer.IOfferAddPanelConfig = {
                offer_StatusId: ConfigGenerator.generateOfferStatusIdConfig(),
                offer_CompletionDate: ConfigGenerator.generateOfferCompletionDateConfig(),
                offer_ExchangeDate: ConfigGenerator.generateOfferExchangeDateConfig(),
                offer_OfferDate: ConfigGenerator.generateOfferOfferDateConfig(),
                offer_Price: ConfigGenerator.generateOfferPriceConfig(),
                offer_PricePerWeek: ConfigGenerator.generateOfferPricePerWeekConfig(),
                offer_SpecialConditions: ConfigGenerator.generateOfferSpecialConditionsConfig()
            };

            return config;
        }

        public static generateOfferEditConfig(): Offer.IOfferEditConfig{
            var addPanelConfig: Offer.IOfferAddPanelConfig = ConfigGenerator.generateOfferAddPanelConfig();
            var config = <Offer.IOfferEditConfig>{};
            angular.extend(config, addPanelConfig, <Offer.IOfferEditConfig> {
                offer_AdditionalSurveyDate : ConfigGenerator.generateOfferAdditionalSurveyDateConfig(),
                offer_MortgageSurveyDate : ConfigGenerator.generateOfferMortgageSurveyDateConfig(),
                offer_Broker: ConfigGenerator.generateOfferBrokerConfig(),
                offer_AdditionalSurveyor: ConfigGenerator.generateOfferAdditionalSurveyorConfig(),
                offer_ContractApproved: ConfigGenerator.generateOfferContractApprovedConfig(),
                offer_Enquiries: ConfigGenerator.generateOfferEnquiriesConfig(),
                offer_Lender: ConfigGenerator.generateOfferLenderConfig(),
                offer_MortgageStatus: ConfigGenerator.generateOfferMortgageStatusConfig(),
                offer_MortgageSurveyStatus: ConfigGenerator.generateOfferMortgageSurveyStatusConfig(),
                offer_SearchStatus: ConfigGenerator.generateOfferSearchStatusConfig(),
                offer_Surveyor: ConfigGenerator.generateOfferSurveyorConfig(),
                offer_ProgressComment: ConfigGenerator.generateOfferCommentConfig()
            });

            return config;
        }

        public static generateOfferViewConfig(): Offer.IOfferViewConfig {
            var config = <Offer.IOfferViewConfig> {
                offer_StatusId: ConfigGenerator.generateOfferStatusIdConfig(),
                offer_CompletionDate: ConfigGenerator.generateOfferCompletionDateConfig(),
                offer_ExchangeDate: ConfigGenerator.generateOfferExchangeDateConfig(),
                offer_OfferDate: ConfigGenerator.generateOfferOfferDateConfig(),
                offer_Price: ConfigGenerator.generateOfferPriceConfig(),
                offer_PricePerWeek: ConfigGenerator.generateOfferPricePerWeekConfig(),
                offer_SpecialConditions: ConfigGenerator.generateOfferSpecialConditionsConfig(),
                offer_AdditionalSurveyDate: ConfigGenerator.generateOfferAdditionalSurveyDateConfig(),
                offer_MortgageSurveyDate: ConfigGenerator.generateOfferMortgageSurveyDateConfig(),
                offer_Broker: ConfigGenerator.generateOfferBrokerConfig(),
                offer_AdditionalSurveyor: ConfigGenerator.generateOfferAdditionalSurveyorConfig(),
                offer_ContractApproved: ConfigGenerator.generateOfferContractApprovedConfig(),
                offer_Enquiries: ConfigGenerator.generateOfferEnquiriesConfig(),
                offer_Lender: ConfigGenerator.generateOfferLenderConfig(),
                offer_MortgageStatus: ConfigGenerator.generateOfferMortgageStatusConfig(),
                offer_MortgageSurveyStatus: ConfigGenerator.generateOfferMortgageSurveyStatusConfig(),
                offer_SearchStatus: ConfigGenerator.generateOfferSearchStatusConfig(),
                offer_Surveyor: ConfigGenerator.generateOfferSurveyorConfig(),
                offer_ProgressComment: ConfigGenerator.generateOfferCommentConfig()
            };

            return config;
        }

        public static generateActivityStatusEditConfig(): Attributes.IActivityStatusEditControlConfig {
            var config: Attributes.IActivityStatusEditControlConfig = {
                activityStatusId: {
                    active: true,
                    required: true,
                    allowedCodes: null
                }
            };

            return config;
        }

        public static generateActivityTypeEditConfig(): Attributes.IActivityTypeEditControlConfig {
            var config: Attributes.IActivityTypeEditControlConfig = {
                type: null
            };

            return config;
        }

        public static generateActivityVendorsConfig(): Attributes.IActivityVendorsControlConfig {
            var config: Attributes.IActivityVendorsControlConfig = {
                contactIds: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }


        public static generateActivityLandlordsConfig(): Attributes.IActivityLandlordsControlConfig {
            var config: Attributes.IActivityLandlordsControlConfig = {
                landlords: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateActivityConfig(): Antares.Activity.IActivityConfig {
            var config: Antares.Activity.IActivityConfig = {
                activityStatus: ConfigGenerator.generateActivityStatusEditConfig(),
                activityType: ConfigGenerator.generateActivityTypeEditConfig(),
                landlords: ConfigGenerator.generateActivityLandlordsConfig(),
                vendors: ConfigGenerator.generateActivityVendorsConfig(),
                askingPrice: ConfigGenerator.generateActivityAskingPriceConfig(),
                shortLetPricePerWeek: ConfigGenerator.generateActivityShortLetPricePerWeekControlConfig()
            };

            return config;
        }

        public static generateActivityViewConfig(): Antares.Activity.IActivityViewConfig {
            var config: Antares.Activity.IActivityViewConfig = {
                activityStatus: ConfigGenerator.generateActivityStatusEditConfig(),
                activityType: ConfigGenerator.generateActivityTypeEditConfig(),
                departments: ConfigGenerator.generateDepartmentViewControlConfig(),
                vendors: ConfigGenerator.generateActivityVendorsConfig(),
                landlords: ConfigGenerator.generateActivityLandlordsConfig(),
                negotiators: ConfigGenerator.generateNegotiatorsViewControlConfig(),
                property: ConfigGenerator.generatePropertyViewControlConfig(),
                shortAskingMonthRent: {},
                shortAskingWeekRent: {},
                shortMatchFlexMonthValue: {},
                shortMatchFlexWeekValue: {},
                shortMatchFlexPercentage: {},
                longAskingMonthRent: {},
                longAskingWeekRent: {},
                longMatchFlexMonthValue: {},
                longMatchFlexWeekValue: {},
                longMatchFlexPercentage: {},
                priceType: {},
                activityPrice: {},
                matchFlexValue: {},
                matchFlexPercentage: {}
            };

            return config;
        }

        public static generateActivityEditConfig(): Antares.Activity.IActivityEditConfig {
            var config: Antares.Activity.IActivityEditConfig = {
                activityStatus: ConfigGenerator.generateActivityStatusEditConfig(),
                activityType: ConfigGenerator.generateActivityTypeEditConfig(),
                landlords: ConfigGenerator.generateActivityLandlordsConfig(),
                vendors: ConfigGenerator.generateActivityVendorsConfig(),
                askingPrice: ConfigGenerator.generateActivityAskingPriceConfig(),
                shortLetPricePerWeek: ConfigGenerator.generateActivityShortLetPricePerWeekControlConfig(),
                departments: ConfigGenerator.generateDepartmentViewControlConfig(),
                negotiators: ConfigGenerator.generateNegotiatorsViewControlConfig(),
                property: ConfigGenerator.generatePropertyViewControlConfig(),
                agreedInitialMarketingPrice: {},
                serviceChargeAmount: {},
                shortAgreedInitialMarketingPrice: {},
                shortKfValuationPrice: {},
                shortVendorValuationPrice: {},
                vendorValuationPrice: {},
                decoration: {},
                groundRentAmount: {},
                groundRentNote: {},
                kfValuationPrice: {},
                longAgreedInitialMarketingPrice: {},
                longKfValuationPrice: {},
                longVendorValuationPrice: {},
                otherCondition: {},
                source: {},
                sourceDescription: {},
                sellingReason: {},
                pitchingThreats: {},
                keyNumber: {},
                accessArrangements: {},
                appraisalMeetingDate: {},
                appraisalMeetingAttendees: {},
                appraisalMeetingInvitation: {},
                disposalType: {}
            };

            return config;
        }

        public static generateActivityAskingPriceConfig(): Attributes.IActivityAskingPriceControlConfig {
            var config: Attributes.IActivityAskingPriceControlConfig = {
                askingPrice: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateActivityShortLetPricePerWeekControlConfig(): Attributes.IActivityShortLetPricePerWeekControlConfig {
            var config: Attributes.IActivityShortLetPricePerWeekControlConfig = {
                shortLetPricePerWeek: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateDepartmentViewControlConfig(): Attributes.IActivityDepartmentsViewControlConfig {
            var config: Attributes.IActivityDepartmentsViewControlConfig = {
                activityDepartments: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateNegotiatorsViewControlConfig(): Attributes.IActivityNegotiatorsControlConfig {
            var config: Attributes.IActivityNegotiatorsControlConfig = {
                contactIds: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generatePropertyViewControlConfig(): Attributes.IPropertyViewControlConfig {
            var config: Attributes.IPropertyViewControlConfig = {
                propertyId: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateFieldConfig(specificData?: any): Antares.Common.Models.Dto.IFieldConfig {
            var config: Antares.Common.Models.Dto.IFieldConfig = {
                active: true,
                required: true
            };

            return angular.extend(config, specificData || {});
        }

        public static generateRequirementDescriptionAddConfig(): Attributes.IRequirementDescriptionEditControlConfig {
            var fieldConfig: Attributes.IRequirementDescriptionEditFieldConfig = {
                active: true,
                required: null
            };

            var config: Attributes.IRequirementDescriptionEditControlConfig = {
                description: fieldConfig,
            }

            return config;
        }

        public static generateRequirementAddConfig(): Requirement.IRequirementAddConfig {
            var config: Requirement.IRequirementAddConfig = {
                requirementType: null,
                requirement_Description: ConfigGenerator.generateRequirementDescriptionAddConfig()
            };

            return config;
        }

        public static generateRequirementViewConfig(): Requirement.IRequirementViewConfig {
            var config: Requirement.IRequirementViewConfig = {
                requirementType: null,
                requirement_Description: ConfigGenerator.generateRequirementDescriptionAddConfig()
            };

            return config;
        }

        public static generateRequirementTypeEditConfig(): Attributes.IRequirementTypeEditControlConfig {
            var config: Attributes.IRequirementTypeEditControlConfig = {
                requirementTypeId: {
                    required: true,
                    active: true
                }
            };

            return config;
        }
    }
}