/// <reference path="../../typings/_all.d.ts" />


module Antares.Services {
    import Commands = Antares.Common.Models.Commands;
    import Dto = Antares.Common.Models.Dto;
    import Enums = Antares.Common.Models.Enums;
	
    export class ChainTransationsService {

        constructor(private activityService: Services.ActivityService, private requirementService: Requirement.RequirementService) {
        }

        public addChain(chain: Dto.IChainTransaction, command: Commands.IChainTransactionCommand, chainType: Enums.OfferChainsType): ng.IHttpPromise<any>{
            command.chainTransactions.push(chain);
            return this.saveChains(command, chainType);
        }

        public editChain(chain: Dto.IChainTransaction, command: Commands.IChainTransactionCommand, chainType: Enums.OfferChainsType): ng.IHttpPromise<any>{
            var targetChain = command.chainTransactions.filter(c => c.id === chain.id)[0];

            angular.copy(chain, targetChain);
            
            return this.saveChains(command, chainType);
        }

        public removeChain(chain: Dto.IChainTransaction, command: Commands.IChainTransactionCommand, chainType: Enums.OfferChainsType): ng.IHttpPromise<any>{
            var chainToRemove = command.chainTransactions.filter(c => c.id === chain.id)[0];

            var index = command.chainTransactions.indexOf(chainToRemove);
            command.chainTransactions.splice(index, 1);
            
            return this.saveChains(command, chainType);
        }
        
        private saveChains(command: Commands.IChainTransactionCommand,  chainType: Enums.OfferChainsType): ng.IHttpPromise<any>{
            if(chainType == Enums.OfferChainsType.Activity){
                var activityCommand = <Commands.Activity.ActivityEditCommand>command;
                return this.activityService.updateActivity(activityCommand);
            } else {
                return this.requirementService.updateRequirement(command);
            }
        }
    }

    angular.module('app').service('chainTransationsService', ChainTransationsService);
}