/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;

    export class ActivityVendorsControlController {
        // binding
        ownerships: Business.Ownership[];
        setVendors: (obj: { vendors: Business.Contact[] }) => void;

        // controller
        vendorContacts: Business.Contact[] = [];

        $onChanges = () => {
            var vendor: Business.Ownership = _.find(this.ownerships, (ownership: Business.Ownership) => {
                return ownership.isVendor();
            });

            if (vendor) {
                this.vendorContacts = vendor.contacts;
                this.setVendors({ vendors: vendor.contacts });
            }
        }
    }

    angular.module('app').controller('ActivityVendorsControlController', ActivityVendorsControlController);
};