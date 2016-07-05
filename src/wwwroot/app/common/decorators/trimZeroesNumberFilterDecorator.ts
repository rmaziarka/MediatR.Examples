/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Decorators {
    export class TrimZeroesNumberFilterDecorator {
        public static decoratorFunction($delegate: Function, $parse: ng.IParseService){
            return (number: number | string, fractionSize?: number | string, trimZeroes?: boolean) =>{
                var parsedNumber: number = typeof number === "string" ? parseFloat(number) : number;
                if (trimZeroes && parsedNumber % 1 === 0) {
                    fractionSize = 0;
                }
                return $delegate(number, fractionSize);
            }
        }
    }
}