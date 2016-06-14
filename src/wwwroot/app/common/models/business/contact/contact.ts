/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Contact {

        id: string;
        firstName: string;
        surname: string;
        title: string;
		company: Business.Company = null;

        constructor(contact?: Dto.IContact, company?: Dto.ICompany) {
            angular.extend(this, contact);
			if (company) {
				this.company = new Business.Company(company);
			}
        }

        public getName() {
            return this.firstName + ' ' + this.surname;
        }
    }
}