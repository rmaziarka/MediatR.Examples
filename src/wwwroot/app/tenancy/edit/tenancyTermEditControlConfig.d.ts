/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Tenancy {
    interface ITenancyTermEditControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        term: Antares.Common.Models.Dto.IFieldConfig;
    }
}