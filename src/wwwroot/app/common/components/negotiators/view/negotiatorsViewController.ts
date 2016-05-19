/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
	import Business = Common.Models.Business;

    export class NegotiatorsViewController {
        public leadNegotiator: Business.ActivityUser;
        public secondaryNegotiators: Business.ActivityUser[];
    }

	angular.module('app').controller('NegotiatorsViewController', NegotiatorsViewController);
}