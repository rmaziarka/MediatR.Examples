/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Commands.Activity {
    import Business = Models.Business;

    export class ActivityEditCommand extends ActivityBaseCommand implements IActivityEditCommand, IChainTransactionCommand {
        id: string;
        chainTransactions: Business.ChainTransaction[] = [];

        constructor(activity: Business.ActivityEditModel) {
            super(activity);
            this.id = activity.id;
            this.chainTransactions = activity.chainTransactions;
        }
    }

    export interface IActivityEditCommand extends IActivityBaseCommand {
        id: string;
    }
}