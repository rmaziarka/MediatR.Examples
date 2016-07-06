/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CompanyContactRelation {
        constructor(public contact: Contact, public company: Company) {
        }
    }
}