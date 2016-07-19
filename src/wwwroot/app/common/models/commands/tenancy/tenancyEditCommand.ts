/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Commands.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyEditCommand implements ITenancyEditCommand {
        tenancyId: string;
        term: TenancyTermCommandPart;

        constructor(tenancy?: Business.TenancyEditModel) {
            if (tenancy) {
                this.tenancyId = tenancy.id;
                this.term = new TenancyTermCommandPart(tenancy);
            }
        }
    }

    export interface ITenancyEditCommand {
        tenancyId: string;
        term: TenancyTermCommandPart
    }
}