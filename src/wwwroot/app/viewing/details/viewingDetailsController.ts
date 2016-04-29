///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Dto = Antares.Common.Models.Dto;
        import Business = Common.Models.Business;

        export class ViewingDetailsController {
            componentId: string;
            activity: Dto.IActivityQueryResult;
            viewing: Dto.IViewing;
            dateOpened: boolean = false;
            attendees: Dto.IContact[];
            startTime: Date;
            endTime: Date;

            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService,
                private $scope: ng.IScope,
                private $q: ng.IQService) {

                componentRegistry.register(this, this.componentId);
            }

            clearViewingDetails = () =>{
                this.viewing = new Business.Viewing();
                this.startTime = new Date();
                this.endTime = new Date();
                this.setSelectedAttendees([]);
                var form = this.$scope["addViewingForm"];
                form.$setPristine();
            }

            getSelectedAttendees = () =>{
                return this.attendees.filter((c: any) =>{ return c.selected });
            }

            isDataValid = (): boolean => {
                var form = this.$scope["addViewingForm"];
                form.$setSubmitted();
                return form.$valid;
            }

            setSelectedAttendees = (itemsToSelect: Array<string>) =>{
                this.attendees.forEach((c: any) =>{ c.selected = false; });
                if (itemsToSelect === undefined || itemsToSelect === null || itemsToSelect.length === 0) {
                    return;
                }

                this.attendees.forEach((c: any) =>{
                    if (itemsToSelect.indexOf(c.id) > -1) {
                        c.selected = true;
                    }
                });
            }

            setActivity = (activity: Dto.IActivityQueryResult) =>{
                this.activity = activity;
            }

            openDate = () =>{
                this.dateOpened = true;
            }

            saveViewing = (requirementId: string) => {
                // TODO Prepare createViewingCommand

                if (!this.isDataValid()) {
                    return this.$q.reject();
                }

//                var createViewingCommand: Dto.IViewing = angular.copy(this.viewing);
//
//                var requirementResource = this.dataAccessService.getRequirementResource();
//
//                return requirementResource
//                    .createViewing({ requirementId: requirementId }, createViewingCommand)
//                    .$promise
//                    .then((viewing: Common.Models.Dto.IViewing) => {
//                        var form = this.$scope["addViewingForm"];
//                        form.$setPristine();
//                    });
            }
        }

        angular.module('app').controller('viewingDetailsController', ViewingDetailsController);
    }
}