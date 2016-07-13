/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Tenancy {
    import Attributes = Antares.Attributes;

    interface ITenancyEditConfig {
        tenancy_Term: ITenancyTermEditControlConfig;
    }
}