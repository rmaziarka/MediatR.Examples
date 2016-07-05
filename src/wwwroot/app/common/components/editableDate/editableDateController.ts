/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class EditableDateController {
        // component binding
        public selectedDate: Date;
        public isRequired: boolean = false;
        public onSave: () => (date: Date) => ng.IPromise<any>;
        public canBeEdited: boolean;

        // component data
        public isDatePickerOpen: boolean = false;
        public date: Date;
        public today: Date = new Date();

        private inEditMode: boolean;

        constructor() {
            this.inEditMode = false;
            this.date = this.selectedDate;
        }

        $onChanges = (changesObj: any) => {
            if (changesObj
                .canBeEdited &&
                changesObj.canBeEdited.currentValue !== changesObj.canBeEdited.previousValue) {
                if (changesObj.canBeEdited.currentValue === false) {
                    this.inEditMode = false;
                }
            }
        }

        public openEditMode = () => {
            this.date = this.selectedDate;
            this.inEditMode = true;
        }

        public save = () => {
            this.onSave()(this.date).then(() => {
                this.inEditMode = false;
            });
        }

        public cancel = () => {
            this.inEditMode = false;
        }

        public openDatePicker = () => {
            this.isDatePickerOpen = true;
        }

        public isBeforeToday = () => {
            return moment(this.selectedDate).isBefore(this.today, 'day');
        }

        public isInEditMode = () => {
            return this.inEditMode && this.canBeEdited;;
        }
    }

    angular.module('app').controller('EditableDateController', EditableDateController);
};