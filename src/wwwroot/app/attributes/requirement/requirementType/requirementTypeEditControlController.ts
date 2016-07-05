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
        private allRequirementTypes: Dto.IRequirementTypeQueryResult[];

        constructor(public requirementService: Requirement.RequirementService) { }

        $onInit = () => {
            this.loadRequirementTypes();
        }

        public changeRequirementType = () => {
            this.onRequirementTypeChanged({ requirementTypeId: this.ngModel });
        }

        loadRequirementTypes = () =>{
            this.requirementService
                .getRequirementTypes("GB")
                .then((requirementTypes: angular.IHttpPromiseCallbackArg<Array<Dto.IRequirementTypeQueryResult>>) => {
                    this.allRequirementTypes = requirementTypes.data;
                });
        }
    }

    angular.module('app').controller('RequirementTypeEditControlController', RequirementTypeEditControlController);
};