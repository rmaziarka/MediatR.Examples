/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Models.Dto;

    export class ActivityEditModel extends ActivityBaseModel {
        chainTransactions: Business.ChainTransaction[] = [];

        constructor(activity?: Dto.IActivity) {
            super(activity);

            if (activity && activity.chainTransactions) {
                this.chainTransactions = activity.chainTransactions.map((chainTransaction: Dto.IChainTransaction) => { return new Business.ChainTransaction(chainTransaction) });
            }
            else {
                this.chainTransactions = [];
            }
        }
    }
}