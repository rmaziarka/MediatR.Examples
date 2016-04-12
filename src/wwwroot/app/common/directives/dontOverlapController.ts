/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class DontOverlapController {
        minValue: Date;
        maxValue: Date;
        rangeList: Array<any>;
        hasMinAttr: boolean;
        hasMaxAttr: boolean;
        minAttrName: string = 'dontOverlapMin';
        maxAttrName: string = 'dontOverlapMax';
        lowestDate: Date = new Date(1000, 1, 1);
        highestDate: Date = new Date(9999, 1, 1);
        getMin = (value?: Date): Date => this.hasMinAttr || !value ? this.minValue : value;
        getMax = (value?: Date): Date => this.hasMaxAttr || !value ? this.maxValue : value;
        isSameDay = (rangeMin:Date, rangeMax:Date, min:Date, max:Date) => {
            return (rangeMin.getTime() === rangeMax.getTime()
                && min
                && max
                && min.getTime() === max.getTime()
                && min.getTime() === rangeMin.getTime());
        }
        isOverlapping = (attrs: ng.IAttributes, value?: any): boolean=> {
            var
                fromField = attrs['fromField'] || 'from',
                toField = attrs['toField'] || 'to';

            if (this.rangeList == undefined || this.rangeList.length === 0) return false;
            for (var i = 0; i < this.rangeList.length; i++) {
                var rangeMin = this.rangeList[i][fromField] ? new Date(this.rangeList[i][fromField]) : this.lowestDate;
                var rangeMax = this.rangeList[i][toField] ? new Date(this.rangeList[i][toField]) : this.highestDate;
                var min = this.getMin(value);
                var max = this.getMax(value);
                var overlaps = ((min || rangeMin) < rangeMax && rangeMin < (max || rangeMax)) || this.isSameDay(rangeMin, rangeMax, min, max);
                if (overlaps) return true;
            }
            return false;
        };

        validate = (attrs: ng.IAttributes, ngModel: ng.INgModelController, value?: any): boolean=> {
            var valid = !this.isOverlapping(attrs, value);
            ngModel.$setValidity('dontOverlap', valid);
            return value;
        };

        attachAttributeObservers = (attrs: ng.IAttributes, ngModel: ng.INgModelController) => {
            var getDate = (value: string): Date => new Date(value.replace(/"/g, ''));

            attrs.$observe('dontOverlap', (value: string) => {
                this.rangeList = JSON.parse(String(value));
                return this.validate(attrs, ngModel);
            });

            /*
                Watch for changes on values of the dependant minimum range field and fire validation.
            */
            attrs.$observe(this.minAttrName, (value: string) => {
                this.minValue = value ? getDate(String(value)) : this.lowestDate;
                return this.validate(attrs, ngModel);
            });

            /*
                Watch for changes on values of the dependent maximum range field and fire validation.
            */
            attrs.$observe(this.maxAttrName, (value: string) => {
                this.maxValue = value ? getDate(String(value)) : this.highestDate;
                return this.validate(attrs, ngModel);
            });
        }

        initModelValidationHandlers = (attrs: ng.IAttributes, ngModel: ng.INgModelController) => {
            this.hasMinAttr = attrs[this.minAttrName] != undefined;
            this.hasMaxAttr = attrs[this.maxAttrName] != undefined;

            /*
                This function is called when model value changes
                Directive needs know which field it is handling - is it minimum range value or maximum.
            */
            var setAndValidate = (value: any): boolean => {
                if (this.hasMinAttr && !this.hasMaxAttr) {
                    this.maxValue = value;
                }
                if (this.hasMaxAttr && !this.hasMinAttr) {
                    this.minValue = value;
                }
                return this.validate(attrs, ngModel, value);
            }

            ngModel.$parsers.push(setAndValidate);
            ngModel.$formatters.push(setAndValidate);
        }
    }

    angular.module('app').controller('dontOverlapController', DontOverlapController);
}