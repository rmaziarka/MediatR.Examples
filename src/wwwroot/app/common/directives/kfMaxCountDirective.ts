/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class KfMaxCount implements ng.IDirective {
        restrict = 'A';
        require = 'ngModel';
        scope = {
            ngModel: '=ngModel',
            maxCount: '=kfMaxCount'
        };
        link: any;

        constructor() {
            this.link = this.unboundLink.bind(this);
        }

        unboundLink(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) {

            ngModel.$validators['kfMaxCount'] = (modelValue: string) => {
                var maxCount: number = parseInt(scope["maxCount"]);
                if (isNaN(maxCount)) {
                    maxCount = 0;
                }
                if (!modelValue) {
                    return true;
                }
                return (modelValue.length <= maxCount);
            }
            scope.$watch('ngModel', (newValue, oldValue) => {
                if ((newValue !== undefined || oldValue !== undefined) && newValue !== oldValue && newValue !== "") {
                    ngModel.$setDirty();
                }
            });
        }

        static factory() {
            var directive = () => {
                return new KfMaxCount();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('kfMaxCount', KfMaxCount.factory());
}