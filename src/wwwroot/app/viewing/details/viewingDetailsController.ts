///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Dto = Antares.Common.Models.Dto;

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
                private dataAccessService: Antares.Services.DataAccessService){

                componentRegistry.register(this, this.componentId);
            }

            setActivity = (activity: Dto.IActivityQueryResult) =>{
                this.activity = activity;
            }

            openDate = () =>{
                this.dateOpened = true;
            }
        }

        angular.module('app').controller('viewingDetailsController', ViewingDetailsController);
    }
}