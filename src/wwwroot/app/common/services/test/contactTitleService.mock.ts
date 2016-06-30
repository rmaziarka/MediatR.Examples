/// <reference path="../../../typings/_all.d.ts" />

module Antares.Mock {
    import Dto = Common.Models.Dto;

    export class ContactTitleServiceMock {

        private contactTitles: any = {};
        private promise: ng.IPromise<Dto.IContactTitle[]> = null;
        private deferred: ng.IDeferred<Dto.IContactTitle[]> = null;

        constructor(private $q: ng.IQService){
            this.deferred = $q.defer();
            this.promise = this.deferred.promise;
        }

        setTitles = (contactTitles:Dto.IContactTitle[]) =>{
            this.contactTitles = contactTitles;
        }

        get = (): ng.IPromise<Dto.IContactTitle[]> =>{
            this.deferred.resolve(this.contactTitles);
            return this.promise;
        };
    }
}