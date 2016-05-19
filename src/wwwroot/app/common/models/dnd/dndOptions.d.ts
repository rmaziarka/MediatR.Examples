/// <reference path="../../../typings/_all.d.ts" />

declare module Antares.Common.Models {
    interface IDndOptions {
        dragStart(event: IDndEvent): void;
        dragEnd(event: IDndEvent): void;
    }
}