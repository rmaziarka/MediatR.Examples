/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    export class KfErrorInterceptor implements ng.IHttpInterceptor {

        constructor(
            private kfMessageService: KfMessageService,
            private $q: ng.IQService) {
        }

        responseError = (rejection: ng.IHttpPromiseCallbackArg<any>) => {
            if (rejection.status === 500) {
                this.kfMessageService.showErrorByCode('COMMON.UNEXPECTED_SERVER_ERROR');
            }

            return this.$q.reject(rejection);
        }

        public static factory(kfMessageService: KfMessageService, $q: ng.IQService): ng.IHttpInterceptor{
            return new KfErrorInterceptor(kfMessageService, $q);
        }
    }
}