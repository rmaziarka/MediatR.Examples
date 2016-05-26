/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class StringGenerator {
        public static generate(length?: number) {
            var chars = "abcdefghijklmnopqrstufwxyzABCDEFGHIJKLMNOPQRSTUFWXYZ1234567890";
            var pwd = _.sample(chars, length || 10);
            return pwd.join("");
        }
    }
}