/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    export class ConfigGenerator {
        public static generateOfferOfferDateConfig(): Attributes.IOfferOfferDateControlConfig{
            var config: Attributes.IOfferOfferDateControlConfig = {
                active : true,
                offerDate : null
            }

            return config;
        }

        public static generateOfferExchangeDateConfig(): Attributes.IOfferExchangeDateControlConfig {
            var config: Attributes.IOfferExchangeDateControlConfig = {
                active: true,
                exchangeDate: null
            }

            return config;
        }

        public static generateOfferCompletionDateConfig(): Attributes.IOfferCompletionDateControlConfig {
            var config: Attributes.IOfferCompletionDateControlConfig = {
                active: true,
                completionDate: null
            }

            return config;
        }

        public static generateOfferAdditionalSurveyDateConfig(): Attributes.IOfferAdditionalSurveyDateControlConfig {
            var config: Attributes.IOfferAdditionalSurveyDateControlConfig = {
                active: true,
                additionalSurveyDate: null
            }

            return config;
        }

        public static generateOfferMortgageSurveyDateConfig(): Attributes.IOfferMortgageSurveyDateControlConfig {
            var config: Attributes.IOfferMortgageSurveyDateControlConfig = {
                active: true,
                mortgageSurveyDate: null
            }

            return config;
        }

        public static generateOfferStatusIdConfig(): Attributes.IOfferStatusControlConfig {
            var config: Attributes.IOfferStatusControlConfig = {
                active: true,
                statusId: null
            }

            return config;
        }

        public static generateOfferPriceConfig(): Attributes.IOfferPriceControlConfig {
            var config: Attributes.IOfferPriceControlConfig = {
                active: true,
                price: null
            }

            return config;
        }

        public static generateOfferPricePerWeekConfig(): Attributes.IOfferPricePerWeekControlConfig {
            var config: Attributes.IOfferPricePerWeekControlConfig = {
                active: true,
                pricePerWeek: null
            }

            return config;
        }

        public static generateOfferSpecialConditionsConfig(): Attributes.IOfferSpecialConditionsControlConfig {
            var config: Attributes.IOfferSpecialConditionsControlConfig = {
                active: true,
                specialConditions: null
            }

            return config;
        }

        public static generateOfferCommentConfig(): Attributes.IOfferProgressCommentControlConfig {
            var config: Attributes.IOfferProgressCommentControlConfig = {
                active: true,
                progressComment: null
            }

            return config;
        }

        public static generateOfferBrokerConfig(): Attributes.IOfferBrokerControlConfig {
            var config: Attributes.IOfferBrokerControlConfig = {
                active: true,
                brokerId: null
            }

            return config;
        }
        public static generateOfferAdditionalSurveyorConfig(): Attributes.IOfferAdditionalSurveyorControlConfig {
            var config: Attributes.IOfferAdditionalSurveyorControlConfig = {
                active: true,
                additionalSurveyorId: null
            }

            return config;
        }

        public static generateOfferLenderConfig(): Attributes.IOfferLenderControlConfig {
            var config: Attributes.IOfferLenderControlConfig = {
                active: true,
                lenderId: null
            }

            return config;
        }

        public static generateOfferSurveyorConfig(): Attributes.IOfferSurveyorControlConfig {
            var config: Attributes.IOfferSurveyorControlConfig = {
                active: true,
                surveyorId: null
            }

            return config;
        }

        public static generateOfferContractApprovedConfig(): Attributes.IOfferContractApprovedControlConfig {
            var config: Attributes.IOfferContractApprovedControlConfig = {
                active: true,
                contractApproved: null
            }

            return config;
        }

        public static generateOfferEnquiriesConfig(): Attributes.IOfferEnquiriesControlConfig {
            var config: Attributes.IOfferEnquiriesControlConfig = {
                active: true,
                enquiriesId: null
            }

            return config;
        }

        public static generateOfferMortgageStatusConfig(): Attributes.IOfferMortgageStatusControlConfig {
            var config: Attributes.IOfferMortgageStatusControlConfig = {
                active: true,
                mortgageStatusId: null
            }

            return config;
        }

        public static generateOfferMortgageSurveyStatusConfig(): Attributes.IOfferMortgageSurveyStatusControlConfig {
            var config: Attributes.IOfferMortgageSurveyStatusControlConfig = {
                active: true,
                mortgageSurveyStatusId: null
            }

            return config;
        }

        public static generateOfferSearchStatusConfig(): Attributes.IOfferSearchStatusControlConfig {
            var config: Attributes.IOfferSearchStatusControlConfig = {
                active: true,
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
            angular.extend(config, addPanelConfig, {
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
                offer_Comment: ConfigGenerator.generateOfferCommentConfig()
            });

            return config;
        }

        public static generateActivityStatusEditConfig(): Attributes.IActivityStatusEditControlConfig {
            var config: Attributes.IActivityStatusEditControlConfig = {
                active: true,
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
                active: true,
                type: null
            };

            return config;
        }

        public static generateActivityVendorsConfig(): Attributes.IActivityVendorsControlConfig {
            var config: Attributes.IActivityVendorsControlConfig = {
                active: true,
                contactIds: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }


        public static generateActivityLandlordsConfig(): Attributes.IActivityLandlordsControlConfig {
            var config: Attributes.IActivityLandlordsControlConfig = {
                active: true,
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
                property: ConfigGenerator.generatePropertyViewControlConfig()
            };

            return config;
        }

        public static generateActivityAskingPriceConfig(): Attributes.IActivityAskingPriceControlConfig {
            var config: Attributes.IActivityAskingPriceControlConfig = {
                active: true,
                askingPrice: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateActivityShortLetPricePerWeekControlConfig(): Attributes.IActivityShortLetPricePerWeekControlConfig {
            var config: Attributes.IActivityShortLetPricePerWeekControlConfig = {
                active: true,
                shortLetPricePerWeek: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateDepartmentViewControlConfig(): Attributes.IActivityDepartmentsViewControlConfig {
            var config: Attributes.IActivityDepartmentsViewControlConfig = {
                active: true,
                activityDepartments: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generateNegotiatorsViewControlConfig(): Attributes.IActivityNegotiatorsControlConfig {
            var config: Attributes.IActivityNegotiatorsControlConfig = {
                active: true,
                contactIds: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
            };

            return config;
        }

        public static generatePropertyViewControlConfig(): Attributes.IPropertyViewControlConfig {
            var config: Attributes.IPropertyViewControlConfig = {
                active: true,
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
                active: true
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
                active: true,
                requirementTypeId: {
                    required: true,
                    active: true
                }
            };

            return config;
        }
    }
}