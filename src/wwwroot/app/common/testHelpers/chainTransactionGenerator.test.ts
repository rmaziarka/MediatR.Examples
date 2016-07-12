/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ChainTransactionGenerator extends TestDataGenerator<Dto.IChainTransaction, Business.ChainTransaction> {
        public generateDto(specificData?: any): Dto.IChainTransaction {
            var chain: Dto.IChainTransaction =  {
                id: this.makeRandom("id"),
                isEnd: true,
                propertyId: this.makeRandom("propertyId"),
                property: null,
                vendor: this.makeRandom("vendor"),
                mortgageId: this.makeRandom("mortgageId"),
                mortgage: null,
                surveyId: this.makeRandom("surveyId"),
                survey: null,
                searchesId: this.makeRandom("searchesId"),
                searches: null,
                enquiriesId: this.makeRandom("enquiriesId"),
                enquiries: null,
                contractAgreedId: this.makeRandom("contractAgreedId"),
                contractAgreed: null,
            };

            return angular.extend(chain, specificData || {});
        };
    }
}