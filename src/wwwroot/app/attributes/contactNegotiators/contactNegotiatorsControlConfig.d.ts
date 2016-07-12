/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IContactNegotiatorsControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        contactIds: Antares.Common.Models.Dto.IFieldConfig;
    }
}