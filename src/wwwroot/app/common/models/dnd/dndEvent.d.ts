/// <reference path="../../../typings/_all.d.ts" />

declare module Antares.Common.Models {
    interface IDndEvent {
        source: IDndEventSource;
        dest: IDndEventDest;
    }
}