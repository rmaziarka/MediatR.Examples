/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class OfferService {
        private url: string = '/api/offers/';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        createOffer = (offerCommand: Business.CreateOfferCommand): ng.IPromise<Dto.IOffer> => {
            return this.$http.post(this.appConfig.rootUrl + this.url, offerCommand)
                .then<Dto.IOffer>((result: ng.IHttpPromiseCallbackArg<Dto.IOffer>) => result.data);
        }

        updateOffer = (offerCommand: Dto.IOffer): ng.IPromise<Dto.IOffer> => {
            return this.$http.put(this.appConfig.rootUrl + this.url, offerCommand)
                .then<Dto.IOffer>((result: ng.IHttpPromiseCallbackArg<Dto.IOffer>) => result.data);
        }

        getOfferTypes = (): ng.IHttpPromise<Array<Dto.IOfferType>> => {
            var url = `${this.url}types`;
            return this.$http.get(this.appConfig.rootUrl + url)
                .then<Array<Dto.IOfferType>>((result: ng.IHttpPromiseCallbackArg<Array<Dto.IOfferType>>) => result.data);                ;
        }
    }

    angular.module('app').service('offerService', OfferService);
};