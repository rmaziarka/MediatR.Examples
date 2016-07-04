/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ConfigGenerator {
        public static generateOfferOfferDateConfig(): Attributes.IOfferOfferDateControlConfig{
            var config: Attributes.IOfferOfferDateControlConfig = {
                offerDate : null
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

		public static generateTextControlConfig(): Attributes.ITextControlConfig {
            var config: Attributes.ITextControlConfig = {
                active: true,
                textArea: Antares.TestHelpers.ConfigGenerator.generateFieldConfig()
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
    }
}