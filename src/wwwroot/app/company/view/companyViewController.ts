/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {
    import Business = Antares.Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class CompanyViewController {
        company: Business.Company;
    }

    angular.module('app').controller('CompanyViewController', CompanyViewController);
};