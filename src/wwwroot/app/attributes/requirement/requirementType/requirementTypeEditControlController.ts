/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;

    export class RequirementTypeEditControlController {
        // bindings 
        public ngModel: string;
        public config: IRequirementTypeEditControlConfig;
        onRequirementTypeChanged: (obj: { requirementTypeId: string }) => void;

        // controller
        private requirementResource: Common.Models.Resources.IRequirementResourceClass;
        private allRequirementTypes: Dto.IRequirementTypeQueryResult[] = [];

        constructor(private dataAccessService: Antares.Services.DataAccessService) { }

        $onInit = () => {
            this.requirementResource = this.dataAccessService.getRequirementResource();
            this.loadRequirementTypes();
        }

        public changeRequirementType = () => {
            this.onRequirementTypeChanged({ requirementTypeId: this.ngModel });
        }

        loadRequirementTypes = () =>{
            this.requirementResource
                .getRequirementTypes({
                    countryCode: "GB"
                })
                .$promise
                .then((requirementTypes: Dto.IRequirementTypeQueryResult[]) => {
                    this.allRequirementTypes = requirementTypes;
                });
        }
    }

    angular.module('app').controller('RequirementTypeEditControlController', RequirementTypeEditControlController);
};