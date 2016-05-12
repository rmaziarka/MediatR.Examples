///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        declare var moment: any;
        import Dto = Antares.Common.Models.Dto;
        import Business = Common.Models.Business;

        export class ViewingAddController {
            componentId: string;
            activity: Dto.IActivityQueryResult;
            viewing: Business.Viewing;
            dateOpened: boolean = false;
            attendees: Dto.IContact[];
            // Any beacuse they are momentjs object
            startTime: any;
            endTime: any;
            requirement: Business.Requirement;
            mode: string = 'edit';

            selectedAttendees: Dto.IContact[] = [];
            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService,
                private $scope: ng.IScope,
                private $q: ng.IQService) {

                componentRegistry.register(this, this.componentId);
            }

            clearViewingAdd = () => {
                this.viewing = new Business.Viewing();
                this.startTime = moment();
                this.endTime = moment();
                this.setSelectedAttendees([]);
                var form: ng.IFormController = this.$scope["addViewingForm"];
                form.$setPristine();
                form.$setUntouched();
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
                this.selectedAttendees = [];
                if (itemsToSelect === undefined || itemsToSelect === null || itemsToSelect.length === 0) {
                    return;
                }

                this.attendees.forEach((c: any) => {
                    if (itemsToSelect.indexOf(c.id) > -1) {
                        c.selected = true;
                        this.selectedAttendees.push(c);
                    }
                });
            }

            setActivity = (activity: Dto.IActivityQueryResult) => {
                this.activity = activity;
            }

            setViewing = (viewing: Business.Viewing) => {
                this.viewing = viewing;
                this.viewing.startDate = new Date(moment(viewing.startDate));
                this.viewing.endDate = new Date(moment(viewing.endDate));
                this.startTime = this.combineDateWithTime(new Date(), new Date(moment(viewing.startDate)));
                this.endTime = this.combineDateWithTime(new Date(), new Date(moment(viewing.endDate)));

                this.activity = <Dto.IActivityQueryResult> {
                    id: viewing.activity.id,
                    propertyName: viewing.activity.property.address.propertyName,
                    propertyNumber: viewing.activity.property.address.propertyNumber,
                    line2: viewing.activity.property.address.line2
                };

                var attendeesIds: string[] = [];
                this.viewing.attendees.forEach((attendee: Dto.IContact) =>{
                    attendeesIds.push(attendee.id);
                });
                this.setSelectedAttendees(attendeesIds);
            }

            openDate = () => {
                this.dateOpened = true;
            }

            saveViewing = () => {
                if (!this.isDataValid()) {
                    return this.$q.reject();
                }

                var viewingResource = this.dataAccessService.getViewingResource();

                if (this.mode === 'add') {
                    var createViewingCommand: Dto.IViewing = this.getCreateViewingCommand();
                    return viewingResource
                        .createViewing(null, createViewingCommand)
                        .$promise
                        .then((viewing: Common.Models.Dto.IViewing) =>{
                            var viewingModel = new Business.Viewing(viewing);
                            this.requirement.viewings.push(viewingModel);
                            this.requirement.groupViewings(this.requirement.viewings);
                        });
                }
                else if (this.mode === 'edit') {
                    var updateViewing: Dto.IViewing = this.getUpdateViewingResource();

                    return viewingResource
                        .update(updateViewing)
                        .$promise
                        .then((viewing: Common.Models.Dto.IViewing) => {
                            this.viewing = angular.copy(new Business.Viewing(viewing), this.viewing);
                            this.requirement.groupViewings(this.requirement.viewings);
                        });
                }
            }

            // replace with Business.Viewing and probably extend it with selected: boolean
            getCreateViewingCommand(): Dto.IViewing {
                var createViewingCommand: Dto.IViewing = angular.copy(this.viewing);
                createViewingCommand.startDate = this.combineDateWithTime(this.viewing.startDate, moment(this.startTime).toDate());
                createViewingCommand.endDate = this.combineDateWithTime(this.viewing.startDate, moment(this.endTime).toDate());
                createViewingCommand.activityId = this.activity.id;
                createViewingCommand.requirementId = this.requirement.id;
                createViewingCommand.attendeesIds = this.getSelectedAttendeesIds();

                return createViewingCommand;
            }

            getUpdateViewingResource(): Dto.IViewing {
                var updateViewingResource: Dto.IViewing = angular.copy(this.viewing);
                updateViewingResource.attendeesIds = this.getSelectedAttendeesIds();
                updateViewingResource.startDate = this.combineDateWithTime(this.viewing.startDate, moment(this.startTime).toDate());
                updateViewingResource.endDate = this.combineDateWithTime(this.viewing.startDate, moment(this.endTime).toDate());

                return updateViewingResource;
            }

            getSelectedAttendeesIds(): string[] {
                return this.attendees.filter((element: any): boolean => {
                    return element.selected;
                }).map((element: any) => {
                    return element.id;
                });
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
        angular.module('app').controller('viewingAddController', ViewingAddController);
    }
}