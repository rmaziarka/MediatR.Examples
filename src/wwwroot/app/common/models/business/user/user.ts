/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class User implements Dto.IUser {
        id: string;
        firstName: string;
        lastName: string;

        constructor(user?: Dto.IUser) {
            if (user) {
                angular.extend(this, user);
            }
        }
    }
}