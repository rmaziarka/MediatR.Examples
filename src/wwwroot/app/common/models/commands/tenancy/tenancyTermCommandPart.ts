/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Commands.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyTermCommandPart {
        startDate: Date;
        endDate: Date;
        agreedRent: number;

        constructor(tenancy?: Business.TenancyEditModel) {
            if (tenancy) {
                this.startDate = Core.DateTimeUtils.createDateAsUtc(tenancy.startDate);
                this.endDate = Core.DateTimeUtils.createDateAsUtc(tenancy.endDate);
                this.agreedRent = tenancy.agreedRent;
            }
        }
    }
}