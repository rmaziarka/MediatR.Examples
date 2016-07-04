/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CompanyContactRelation {
        constructor(public contact: Dto.IContact, public company: Dto.ICompany) {
        }
    }
}