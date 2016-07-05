module Antares.Common.Decorators {
    export interface IFilterNumberWithTrimZeroes extends ng.IFilterNumber {
        (value: number | string, fractionSize?: number | string, trimZeroes?: boolean): string;
    }
}