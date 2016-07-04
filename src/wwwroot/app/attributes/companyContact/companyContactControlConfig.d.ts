/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface ICompanyContactControlConfig extends Common.Models.Dto.IControlConfig {
		contact: Common.Models.Dto.IFieldConfig;
		company: Common.Models.Dto.IFieldConfig;
    }
}