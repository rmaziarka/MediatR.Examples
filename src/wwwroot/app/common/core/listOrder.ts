/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class ListOrder {
        constructor(public name: string, public descending?: boolean, public nullOnEnd?: boolean) { }
    }
}