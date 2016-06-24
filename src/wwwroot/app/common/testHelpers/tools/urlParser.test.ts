/// <reference path="../../../typings/_all.d.ts" />

module Antares.TestHelpers {
    export class UrlParser {
        public static getParameter = (url: string, key: string) => {
            var results = new RegExp('[\?&]' + key + '=([^&#]*)').exec(url);
            if (results == null) {
                return null;
            }
            else {
                return results[1] || 0;
            }
        }
    }
}