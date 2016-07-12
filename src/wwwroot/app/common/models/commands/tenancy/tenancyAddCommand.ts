/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Commands.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyAddCommand implements ITenancyAddCommand {
        activityId: string;
        requirementId: string;

        constructor(tenancy?: Business.TenancyEditModel) {
            if (tenancy) {
                this.activityId = tenancy.activity.id;
                this.requirementId = tenancy.requirement.id;
            }
        }
    }

    export interface ITenancyAddCommand {
        activityId: string;
        requirementId: string;
    }
}