/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;
   
	export class ContactNegotiatorsViewControlController {
	    public contactId: string;
        public leadNegotiator: Business.ContactUser;
        public secondaryNegotiators: Business.ContactUser[];


        constructor(
            private $scope: ng.IScope,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService) {
			
        }

    }

angular.module('app').controller('ContactNegotiatorsViewControlController', ContactNegotiatorsViewControlController);
}