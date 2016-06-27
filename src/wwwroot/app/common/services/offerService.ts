/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;
    import CreateOfferCommand = Common.Models.Business.CreateOfferCommand;

    export class OfferService {
        private url: string = '/api/offers/';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        createOffer = (offerCommand: CreateOfferCommand): ng.IPromise<Dto.IOffer> =>{
            return this.$http.post(this.appConfig.rootUrl + this.url, offerCommand)
                .then<Dto.IOffer>((result: ng.IHttpPromiseCallbackArg<Dto.IOffer>) => result.data);
        }
    }

    angular.module('app').service('offerService', OfferService);
};