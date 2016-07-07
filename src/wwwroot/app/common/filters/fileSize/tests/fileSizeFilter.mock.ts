/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Mock.FileSize {

    export function generate() {

        var fileSizeFilter = jasmine.createSpy("fileSizeFilter");

        beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) =>{
            angular.mock.module(($provide: any) =>{
                $provide.value('fileSizeFilter', fileSizeFilter);
            });
        }));

        return fileSizeFilter;
    }
}
