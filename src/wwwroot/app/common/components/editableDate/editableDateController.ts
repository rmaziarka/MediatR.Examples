/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class EditableDateController {
        // component binding
        public selectedDate: Date;
        //public onSave: (id: String, date: Date) => void;
        public onSave: () => (date: Date) => void;

        // component data
        public inEditMode: boolean;
        public isDatePickerOpen: boolean = false;
        public date: Date;

        private activityUserResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityUserResource>;

        constructor(private dataAccessService: Services.DataAccessService) {
            this.inEditMode = false;
            this.date = this.selectedDate;

            this.activityUserResource = dataAccessService.getActivityUserResource();
        }

        public openEditMode = () => {
            this.inEditMode = true;
        }

        public save = () => {
            //this.inEditMode = false;
            //this.selectedDate = this.date;
            //this.onSave(this.id, this.selectedDate);
            this.onSave()(this.date);
        }

        public cancel = () => {
            this.date = this.selectedDate;
            this.inEditMode = false;
        }

        public openDatePicker = () => {
            this.isDatePickerOpen = true;
        }
    }

    angular.module('app').controller('EditableDateController', EditableDateController);
};