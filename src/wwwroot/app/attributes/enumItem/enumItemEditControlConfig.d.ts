/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IEnumSelectEditControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        enumItem: IEnumItemEditFieldConfig;
    }
}