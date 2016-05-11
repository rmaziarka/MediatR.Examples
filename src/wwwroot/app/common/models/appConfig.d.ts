/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Common.Models {
    interface IAppConfig {
        rootUrl: string;
        fileChunkSizeInBytes: number;
    }
}