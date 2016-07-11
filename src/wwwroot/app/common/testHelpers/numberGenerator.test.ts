/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    export class NumberGenerator {
        public static generate() {
            return Math.round(Math.random()*1000) + 1;
        }
    }
}