module Antares.Common.Models.Dto {
    export class Contact {

        id: string;
        firstName: string;
        surname: string;
        title: string;

        constructor(contact?: IContact) {
            angular.extend(this, contact);
        }

        public getName() {
            return this.firstName + ' ' + this.surname;
        }
    }
}