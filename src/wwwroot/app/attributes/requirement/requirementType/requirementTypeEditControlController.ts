/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;

    export class RequirementTypeEditControlController {
        // bindings 
        public ngModel: string;
        public config: IRequirementTypeEditControlConfig;
        onRequirementTypeChanged: (obj: { requirementTypeId: string }) => void;

        // controller
        private allRequirementTypes: Dto.IEnumItem[] = [];

        constructor(private enumService: Services.EnumService) { }

        $onInit = () => {
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        public getRequirementTypes = () => {
            if (!(this.config && this.config.requirementTypeId)) {
                return [];
            }

            if (!this.config.requirementTypeId.allowedCodes) {
                return this.allRequirementTypes;
            }

            return <Dto.IEnumItem[]>_(this.allRequirementTypes).indexBy('code').at(this.config.requirementTypeId.allowedCodes).value();
        }

        public changeRequirementType = () => {
            this.onRequirementTypeChanged({ requirementTypeId: this.ngModel });
        }

        private onEnumLoaded = (result: Dto.IEnumDictionary) => {
            this.allRequirementTypes = result[Dto.EnumTypeCode.RequirementType];
        }
    }

    angular.module('app').controller('RequirementTypeEditControlController', RequirementTypeEditControlController);
};