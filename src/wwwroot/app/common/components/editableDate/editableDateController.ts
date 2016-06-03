/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class EditableDateController {
        // component binding
        public selectedDate: Date;
        public isRequired: boolean = false;
        public onSave: () => (date: Date) => void;

        // component data
        public inEditMode: boolean;
        public isDatePickerOpen: boolean = false;
        public date: Date;

        public today: Date = new Date();

        constructor() {
            this.inEditMode = false;
            this.date = this.selectedDate;
        }

        public openEditMode = () => {
            this.inEditMode = true;
        }

        public save = () => {
            this.inEditMode = false;
            this.onSave()(this.date);
        }

        public cancel = () => {
            this.date = this.selectedDate;
            this.inEditMode = false;
        }

        public openDatePicker = () => {
            this.isDatePickerOpen = true;
        }

        public isBeforeToday = () => {
            return moment(this.date).isBefore(this.today, 'day');
        }
    }

    angular.module('app').controller('EditableDateController', EditableDateController);
};