/// <reference path="../../typings/_all.d.ts" />
/// <reference path="../../common/services/dataaccessservice.ts" />

module Antares {
    export module Requirement {
        export module View {
            export class RequirementViewController {
                static $inject = ['dataAccessService', '$state'];

                isLoading: boolean = true;
                hasError: boolean = false;
                errorStatus: number;
                requirement: any;

                constructor(
                    private dataAccessService: Antares.Services.DataAccessService,
                    $state: angular.ui.IState){

                    var requirementId = $state.params.requirementId;
                    this.requirement = dataAccessService.getRequirementResource().get({ id: requirementId });
                    this.requirement.$promise
                        .catch((response) =>{
                            this.errorStatus = response.status;
                            this.hasError = true;
                        })
                        .finally(() =>{
                            this.isLoading = false;
                        });
                }
            }

            angular.module('app').controller('requirementViewController', RequirementViewController);
        }
    }
}