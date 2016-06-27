/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ConfigGenerator {
        public static generateActivityStatusEditConfig(): Attributes.IActivityStatusEditControlConfig {
            var config: Attributes.IActivityStatusEditControlConfig = {
                active: true,
                activityStatusId: null
            };

            return config;
        }

        public static generateOfferStatusEditConfig(): Attributes.IOfferStatusEditControlConfig {
            var config: Attributes.IOfferStatusEditControlConfig = {
                active: true,
                statusId: null
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
    }
}