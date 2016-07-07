/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class User implements Dto.IUser {
        id: string;
        firstName: string;
        lastName: string;
        departmentId: string;
        department: Department;

        constructor(user?: Dto.IUser) {
            if (user) {
                angular.extend(this, user);
            }
        }

        public getName() {
            return ((this.firstName || '') + ' ' + (this.lastName || '')).trim();
        }
    }
}