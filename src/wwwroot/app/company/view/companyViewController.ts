/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {
    export class CompanyViewController {
        name: string;

        constructor() {
            this.name = 'Marta';
        }

        submitButton() {
            this.name = 'Umar';
        }
    }

    angular.module('app').controller('CompanyViewController', CompanyViewController);
};