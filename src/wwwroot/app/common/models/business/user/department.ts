/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Department implements Dto.IDepartment {
        id: string;
        name: string;
        countryId: string;

        constructor(department?: Dto.IDepartment) {
            if (department) {
                angular.extend(this, department);
            }
        }
    }
}