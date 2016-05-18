/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class DepartmentUser extends User implements Dto.IDepartmentUser {
        department: string;
    }
}