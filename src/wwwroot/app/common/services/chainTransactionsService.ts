/// <reference path="../../typings/_all.d.ts" />


module Antares.Services {
    import Commands = Common.Models.Commands;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
	
    export class ChainTransationsService {

        constructor(private activityService: Services.ActivityService, private requirementService: Requirement.RequirementService) {
        }

        public addChain(chain: Dto.IChainTransaction, command: Commands.IChainTransactionCommand, chainType: Enums.OfferChainsType): ng.IHttpPromise<Dto.IActivity | Dto.IRequirement>{
            command.chainTransactions.push(chain);
            return this.saveChains(command, chainType);
        }

        public editChain(chain: Dto.IChainTransaction, command: Commands.IChainTransactionCommand, chainType: Enums.OfferChainsType): ng.IHttpPromise<Dto.IActivity | Dto.IRequirement>{
            var targetChain = command.chainTransactions.filter(c => c.id === chain.id)[0];

            angular.copy(chain, targetChain);
            
            return this.saveChains(command, chainType);
        }

        public removeChain(chain: Dto.IChainTransaction, command: Commands.IChainTransactionCommand, chainType: Enums.OfferChainsType): ng.IHttpPromise<Dto.IActivity | Dto.IRequirement>{
            var chainToRemove = command.chainTransactions.filter(c => c.id === chain.id)[0];

            var index = command.chainTransactions.indexOf(chainToRemove);
            command.chainTransactions.splice(index, 1);
            
            return this.saveChains(command, chainType);
        }
        
        private saveChains(command: Commands.IChainTransactionCommand,  chainType: Enums.OfferChainsType): ng.IHttpPromise<Dto.IActivity | Dto.IRequirement>{
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