/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class EditableDateController {
        // component binding
        public selectedDate: Date;

        // component data
        public inEditMode: boolean;
        public isDatePickerOpen: boolean = false;
        public date: Date;

        constructor() {
            this.inEditMode = false;
            this.date = this.selectedDate;
        }

        public openEditMode = () => {
            this.inEditMode = true;
        }

        public save = () => {
            // TODO: save data
            this.inEditMode = false;
            this.selectedDate = this.date;
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