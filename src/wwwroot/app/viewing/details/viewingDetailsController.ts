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
            // Any beacuse they are momentjs object
            startTime: any;
            endTime: any;
            requirement: Business.Requirement;

            selectedAttendees: Dto.IContact[] = [];
            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService,
                private $scope: ng.IScope,
                private $q: ng.IQService) {

                componentRegistry.register(this, this.componentId);
            }

            clearViewingDetails = () => {
                this.viewing = new Business.Viewing();
                this.startTime = new Date();
                this.endTime = new Date();
                this.setSelectedAttendees([]);
                var form = this.$scope["addViewingForm"];
                form.$setPristine();
            }

            getSelectedAttendees = () => {
                return this.attendees.filter((c: any) => { return c.selected });
            }

            isDataValid = (): boolean => {
                var form = this.$scope["addViewingForm"];
                form.$setSubmitted();
                return form.$valid;
            }

            setSelectedAttendees = (itemsToSelect: Array<string>) => {
                this.attendees.forEach((c: any) => { c.selected = false; });
                if (itemsToSelect === undefined || itemsToSelect === null || itemsToSelect.length === 0) {
                    return;
                }

                this.attendees.forEach((c: any) => {
                    if (itemsToSelect.indexOf(c.id) > -1) {
                        c.selected = true;
                    }
                });
            }

            setActivity = (activity: Dto.IActivityQueryResult) => {
                this.activity = activity;
            }

            openDate = () => {
                this.dateOpened = true;
            }

            saveViewing = () => {
                if (!this.isDataValid()) {
                    return this.$q.reject();
                }

                var createViewingCommand: Dto.IViewing = this.getCreateViewingCommand();

                var viewingResource = this.dataAccessService.getViewingResource();

                return viewingResource
                    .createViewing(null, createViewingCommand)
                    .$promise
                    .then((viewing: Common.Models.Dto.IViewing) => {
                        var viewingModel = new Business.Viewing(viewing);
                        this.requirement.viewings.push(viewingModel);
                        this.requirement.groupViewings(this.requirement.viewings);

                        var form = this.$scope["addViewingForm"];
                        form.$setPristine();
                    });
            }

            // replace with Business.Viewing and probably extend it with selected: boolean
            getCreateViewingCommand(): Dto.IViewing {
                var createViewingCommand: Dto.IViewing = angular.copy(this.viewing);
                createViewingCommand.startDate = this.combineDateWithTime(this.viewing.startDate, this.startTime.toDate());
                createViewingCommand.endDate = this.combineDateWithTime(this.viewing.startDate, this.endTime.toDate());
                createViewingCommand.activityId = this.activity.id;
                createViewingCommand.requirementId = this.requirement.id;

                createViewingCommand.attendeesIds = this.attendees.filter((element: any): boolean => {
                    return element.selected;
                }).map((element: any) => {
                    return element.id;
                });

                return createViewingCommand;
            }

            combineDateWithTime(date: Date | string, time: Date): Date {
                return new Date(
                    (<Date>date).getFullYear(),
                    (<Date>date).getMonth(),
                    (<Date>date).getDate(),
                    time.getHours(),
                    time.getMinutes(),
                    time.getSeconds(),
                    time.getMilliseconds()
                );
            }

            attendeeClicked(attendee: Dto.IContact): void {
                var index: number = this.selectedAttendees.indexOf(attendee);
                if (index > -1) {
                    this.selectedAttendees.splice(index, 1);
                } else {
                    this.selectedAttendees.push(attendee);
                }
                this.selectedAttendees = this.selectedAttendees.slice();
            }
        }
        angular.module('app').controller('viewingDetailsController', ViewingDetailsController);
    }
}