/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IActivityVendorsControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        contactIds: Antares.Common.Models.Dto.IFieldConfig;
    }
}