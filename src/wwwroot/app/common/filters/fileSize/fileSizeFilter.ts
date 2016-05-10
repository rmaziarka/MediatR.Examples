/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Filters {

    declare var filesize: Filesize.IFilesize;

    export class FileSizeFilter {
        constructor(private filesize: Filesize.IFilesize) {
        }

        transform = (value: number): string => {
            return angular.isNumber(value) ? this.filesize(value, { round: 1 }) : null;
        };

        static getInstance() {
            return (value: number) => {
                var filter = new FileSizeFilter(filesize);
                return filter.transform(value);
            };
        }
    }

    angular.module('app').filter('fileSize', FileSizeFilter.getInstance);
}