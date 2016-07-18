/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface ISelectUserEditControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        user: Antares.Common.Models.Dto.IFieldConfig;
    }
}