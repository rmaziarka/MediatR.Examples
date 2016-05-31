/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;

    export class RequirementCardController {
        requirement: Business.Requirement;
    }

    angular.module('app').controller('requirementCardController', RequirementCardController);
};